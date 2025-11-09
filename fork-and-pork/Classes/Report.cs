namespace fork_and_pork.Classes;

public enum Grade
{
    Terrible, Bad, Ok, Normal, Great
}

public class Report
{
    public DateTime StartDate { get; private set; }
    public DateTime FinishDate { get; private set; }
    public string Notes { get; private set; }
    public Grade Grade { get; private set; }

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