namespace fork_and_pork.Classes;

public enum Grade
{
    Terrible,
    Bad,
    Ok,
    Normal,
    Great
}

public class Report
{
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }

    private string _notes;

    public string Notes
    {
        get { return _notes; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("MenuItem Name cannot be empty or just whitespace.");
            }

            _notes = value;
        }
    }

    public Grade Grade { get; set; }

    public Report(DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        StartDate = startDate;
        FinishDate = finishDate;
        Notes = notes;
        Grade = grade;
        ObjectStore.Add(this);
    }

    public static Report SubmitReport(DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        return new Report(startDate, finishDate, notes, grade);
    }
}