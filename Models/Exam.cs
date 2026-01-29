namespace JalgrataEksamDotNet.Models;

// Database model for an exam row.
public class Exam
{
    public int Id { get; set; }

    // Legacy XML id to keep the original number visible in the table/export.
    public int LegacyId { get; set; }

    public DateTime ExamDate { get; set; }

    public string Location { get; set; } = string.Empty;

    public string ExaminerName { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public int DurationHours { get; set; }

    public string Email { get; set; } = string.Empty;
}
