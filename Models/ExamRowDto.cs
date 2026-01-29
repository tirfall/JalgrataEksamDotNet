namespace JalgrataEksamDotNet.Models;

// DTO for API responses (keeps naming close to the UI labels).
public class ExamRowDto
{
    public int Id { get; set; }
    public string ExamDate { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ExaminerName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public int DurationHours { get; set; }
    public string Email { get; set; } = string.Empty;
}
