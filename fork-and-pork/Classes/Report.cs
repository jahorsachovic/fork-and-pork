using System.ComponentModel.DataAnnotations;

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
    private DateTime _startDate;

    [NotFutureDate]
    public DateTime StartDate
    {
        get { return _startDate; }
        set
        {
            PropertyValidator.Validate(this, value);
            _startDate = value;
        }
    }

    private DateTime _finishDate;

    [NotFutureDate]
    public DateTime FinishDate
    {
        get { return _finishDate; }
        set
        {
            PropertyValidator.Validate(this, value);
            _startDate = value;
        }
    }

    private string _notes;

    [Required(ErrorMessage = "MenuItem Name cannot be empty or just whitespace.")]
    public string Notes
    {
        get { return _notes; }
        set
        {
            PropertyValidator.Validate(this, value);
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