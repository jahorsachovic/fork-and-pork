namespace fork_and_pork.Classes;

public enum Grade
{
    Terrible, Bad, Ok, Normal, Great
}

public class Report
{
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public string Notes { get; set; }
    public Grade Grade { get; set; }

    public static Report SubmitReport(DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        return new Report 
        { 
            StartDate = startDate,
            FinishDate = finishDate,
            Notes = notes,
            Grade = grade
        };
    }
}