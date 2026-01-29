using System.Globalization;
using System.Xml.Linq;
using JalgrataEksamDotNet.Models;

namespace JalgrataEksamDotNet.Data;

// Loads the initial exam list from the XML file into SQLite if the DB is empty.
public static class SeedData
{
    public static void EnsureSeeded(AppDbContext db, string xmlPath)
    {
        if (db.Exams.Any())
        {
            return;
        }

        if (!File.Exists(xmlPath))
        {
            return;
        }

        var document = XDocument.Load(xmlPath);
        var exams = document.Root?.Elements("eksam") ?? Enumerable.Empty<XElement>();

        foreach (var exam in exams)
        {
            db.Exams.Add(ParseExam(exam));
        }

        db.SaveChanges();
    }

    public static Exam ParseExam(XElement exam)
    {
        var nameNode = exam.Element("nimi");
        return new Exam
        {
            LegacyId = ParseInt(exam.Element("id")?.Value),
            ExamDate = ParseDate(exam.Element("eksamiaeg")?.Value),
            Location = exam.Element("koht")?.Value ?? string.Empty,
            ExaminerName = nameNode?.Element("eksamineerijanimi")?.Value ?? string.Empty,
            StudentName = nameNode?.Element("opilanenimi")?.Value ?? string.Empty,
            DurationHours = ParseInt(exam.Element("kestvus")?.Value),
            Email = exam.Attribute("email")?.Value ?? string.Empty
        };
    }

    private static int ParseInt(string? value)
    {
        return int.TryParse(value, out var number) ? number : 0;
    }

    private static DateTime ParseDate(string? value)
    {
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date))
        {
            return date;
        }

        return DateTime.MinValue;
    }
}
