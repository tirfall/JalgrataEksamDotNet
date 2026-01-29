using System.Globalization;
using System.Text;
using System.Text.Json;
using JalgrataEksamDotNet.Data;
using JalgrataEksamDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JalgrataEksamDotNet.Controllers.Api;

// API endpoints for table data and admin-managed JSON export/add/delete.
[ApiController]
[Route("api/exams")]
public class ExamsApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public ExamsApiController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/exams?name=&date=&sort=&order=
    [HttpGet]
    public async Task<IActionResult> GetTable([FromQuery] string? name, [FromQuery] string? date, [FromQuery] string? sort, [FromQuery] string? order)
    {
        var query = _db.Exams.AsNoTracking();

        // Filtreerime nime järgi (eksamineerija või õpilane).
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(exam =>
                exam.ExaminerName.Contains(name) ||
                exam.StudentName.Contains(name));
        }

        // Filtreerime kuupäeva järgi (yyyy-MM-dd). Kui parsimine ebaõnnestub, jätame filtri vahele.
        if (!string.IsNullOrWhiteSpace(date) && DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedDate))
        {
            var nextDay = parsedDate.Date.AddDays(1);
            query = query.Where(exam => exam.ExamDate >= parsedDate.Date && exam.ExamDate < nextDay);
        }

        // Sorteerime valitud veeru järgi.
        var sortKey = (sort ?? "id").ToLowerInvariant();
        var sortOrder = (order ?? "asc").ToLowerInvariant() == "desc" ? "desc" : "asc";

        query = sortKey switch
        {
            "eksamiaeg" => sortOrder == "asc" ? query.OrderBy(exam => exam.ExamDate) : query.OrderByDescending(exam => exam.ExamDate),
            "koht" => sortOrder == "asc" ? query.OrderBy(exam => exam.Location) : query.OrderByDescending(exam => exam.Location),
            "eksamineerijanimi" => sortOrder == "asc" ? query.OrderBy(exam => exam.ExaminerName) : query.OrderByDescending(exam => exam.ExaminerName),
            "opilanenimi" => sortOrder == "asc" ? query.OrderBy(exam => exam.StudentName) : query.OrderByDescending(exam => exam.StudentName),
            "kestvus" => sortOrder == "asc" ? query.OrderBy(exam => exam.DurationHours) : query.OrderByDescending(exam => exam.DurationHours),
            "email" => sortOrder == "asc" ? query.OrderBy(exam => exam.Email) : query.OrderByDescending(exam => exam.Email),
            _ => sortOrder == "asc" ? query.OrderBy(exam => exam.LegacyId) : query.OrderByDescending(exam => exam.LegacyId)
        };

        // Teisendame andmebaasi read API jaoks sobivasse DTO-sse.
        var rows = await query.Select(exam => new ExamRowDto
        {
            Id = exam.LegacyId,
            ExamDate = exam.ExamDate == DateTime.MinValue ? string.Empty : exam.ExamDate.ToString("yyyy-MM-dd HH:mm"),
            Location = exam.Location,
            ExaminerName = exam.ExaminerName,
            StudentName = exam.StudentName,
            DurationHours = exam.DurationHours,
            Email = exam.Email
        }).ToListAsync();

        // Tagastame tabeli andmed koos filtri/sort info'ga.
        return Ok(new
        {
            data = rows,
            sort = sortKey,
            order = sortOrder,
            filters = new { name = name ?? string.Empty, date = date ?? string.Empty }
        });
    }

    // GET /api/exams/export-json
    [HttpGet("export-json")]
    public async Task<IActionResult> ExportJson()
    {
        // JSON eksport kogu tabelist.
        var rows = await _db.Exams.AsNoTracking().OrderBy(exam => exam.LegacyId).ToListAsync();
        var payload = rows.Select(exam => new ExamRowDto
        {
            Id = exam.LegacyId,
            ExamDate = exam.ExamDate == DateTime.MinValue ? string.Empty : exam.ExamDate.ToString("yyyy-MM-dd HH:mm"),
            Location = exam.Location,
            ExaminerName = exam.ExaminerName,
            StudentName = exam.StudentName,
            DurationHours = exam.DurationHours,
            Email = exam.Email
        });

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        var bytes = Encoding.UTF8.GetBytes(json);
        return File(bytes, "application/json", "eksamid.json");
    }

    // POST /api/exams (admin only)
    [HttpPost]
    public async Task<IActionResult> CreateExam([FromBody] ExamRowDto payload)
    {
        // Admin kontroll enne kirjutamist.
        if (!IsAdmin())
        {
            return Forbid();
        }

        // Loome uue eksami kirje.
        var exam = new Exam
        {
            LegacyId = payload.Id,
            ExamDate = ParseDate(payload.ExamDate),
            Location = payload.Location,
            ExaminerName = payload.ExaminerName,
            StudentName = payload.StudentName,
            DurationHours = payload.DurationHours,
            Email = payload.Email
        };

        _db.Exams.Add(exam);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Eksami lisamine õnnestus." });
    }

    // DELETE /api/exams/{id} (admin only)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteExam(int id)
    {
        // Admin kontroll enne kustutamist.
        if (!IsAdmin())
        {
            return Forbid();
        }

        var exam = await _db.Exams.FirstOrDefaultAsync(item => item.LegacyId == id);
        if (exam == null)
        {
            return NotFound(new { error = "Eksamit ei leitud." });
        }

        _db.Exams.Remove(exam);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Eksami kustutamine õnnestus." });
    }

    // Admin olek sessioonist.
    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("IsAdmin") == "true";
    }

    private static DateTime ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DateTime.MinValue;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date))
        {
            return date;
        }

        return DateTime.MinValue;
    }
}