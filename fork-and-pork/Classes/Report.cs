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
    private Employee _employee;

    public Restaurant GetRestaurant()
    {
        return _restaurant;
    }

    public Employee GetInspector()
    {
        return _employee;
    }

    public static Report SubmitReport(Restaurant restaurant, Employee employee, DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        return new Report(restaurant, employee, startDate, finishDate, notes, grade);
    }

    public void DeleteReport()
    {
        _restaurant.RemoveReport(this);
        if (_employee.Inspector is InspectorRole inspector)
        {
            inspector.RemoveReport(this);
        }
        ObjectStore.Delete(this);
    }

    private Report(Restaurant restaurant, Employee employee, DateTime startDate, DateTime finishDate, string notes, Grade grade)
    {
        _restaurant = restaurant;
        _employee = employee;
        _restaurant.AddReport(this);

        if (employee.Inspector is not InspectorRole inspector)
        {
            throw new ArgumentException("Employee is not an inspector.");
        }
        inspector.AddReport(this);

        StartDate = startDate;
        FinishDate = finishDate;
        Notes = notes;
        Grade = grade;
        ObjectStore.Add(this);

    }

}
