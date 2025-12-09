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
    // Attributes
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
    
    // Associations

    private Restaurant _restaurant;
    private Inspector _inspector;

    public Restaurant GetRestaurant()
    {
        return _restaurant;
    }

    public Inspector GetInspector()
    {
        return _inspector;
    }
    
    public static Report SubmitReport(Restaurant restaurant, Inspector inspector, DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        return new Report(restaurant, inspector, startDate, finishDate, notes, grade);
    }

    public void DeleteReport()
    {
        _restaurant.RemoveReport(this);
        _inspector.RemoveReport(this);
        ObjectStore.Delete(this);
    }

    private Report(Restaurant restaurant, Inspector inspector, DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        _restaurant = restaurant;
        _inspector = inspector;
        _restaurant.AddReport(this);
        _inspector.AddReport(this);

        StartDate = startDate;
        FinishDate = finishDate;
        Notes = notes;
        Grade = grade;
        ObjectStore.Add(this);
        
    }

}