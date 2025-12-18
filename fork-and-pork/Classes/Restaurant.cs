using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public abstract class Restaurant
{
    private Address _address;

    [Required]
    public Address Address
    {
        get { return _address;}
        set
        {
            PropertyValidator.Validate(this, value);
            var context = new ValidationContext(value);
            Validator.ValidateObject(value, context, validateAllProperties: true);
            _address = value;
        }
    }

    // Derived Attribute
    public int NumberOfEmployees => _employees.Count;

    // Complex Attribute
    private Dictionary<DayOfWeek, (TimeOnly, TimeOnly)> _workingHours;
    [Required]
    public Dictionary<DayOfWeek, (TimeOnly, TimeOnly)> WorkingHours
    {
        get { return _workingHours; }
        set {
            PropertyValidator.Validate(this, value);
            _workingHours = value;
        }
    }

    // Associations
    private Dictionary<string, Employee> _employees;

    public Dictionary<string, Employee> GetEmployees()
    {
        return new Dictionary<string, Employee>(_employees);
    }

    public void AddEmployee(Employee emp)
    {
        if (_employees.ContainsKey(emp.Email.Address)) return;
        _employees.Add(emp.Email.Address, emp);
        emp.SetRestaurant(this);
    }

    public void RemoveEmployee(Employee emp)
    {
        _employees.Remove(emp.Email.Address);
        if (emp.GetRestaurant() != this) return;
        emp.RemoveFromRestaurant();
    }

    private List<Report> _reports;

    public void AddReport(Report report)
    {
        if(report.GetRestaurant() != this) throw new ArgumentException("Cannot assign report to another restaurant.");
        _reports.Add(report);
    }

    public void RemoveReport(Report report)
    {
        _reports.Remove(report);
    }

    public List<Report> GetReports()
    {
        return new List<Report>(_reports);
    }

    public Restaurant()
    {
        Address = new Address();
        WorkingHours = new Dictionary<DayOfWeek, (TimeOnly, TimeOnly)>();
        _employees = new Dictionary<string, Employee>();
        _reports = new List<Report>();
    }


    public DineInService? DineIn { get; private set; }
    public DeliveryService? Delivery { get; private set; }

    public void EnableDineIn()
    {
        if (DineIn != null) return; // Already enabled
        DineIn = new DineInService();
    }

    public void EnableDelivery(float tax, float radius)
    {
        if (Delivery != null) return; // Already enabled
        Delivery = new DeliveryService(tax, radius);
    }

    public void DisableDineIn()
    {
        if (DineIn == null) return;
        DineIn.Delete();
        DineIn = null;
    }

    public void DisableDelivery()
    {
        if (Delivery == null) return;
        Delivery.Delete();
        Delivery = null;
    }

    public void DeleteRestaurant()
    {
        DisableDineIn();
        DisableDelivery();
        ObjectStore.Delete(this);
    }
}
