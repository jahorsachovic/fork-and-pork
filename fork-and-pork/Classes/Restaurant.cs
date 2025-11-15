using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Restaurant
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

    private Dictionary<DayOfWeek, (TimeOnly, TimeOnly)> _workingHours;
    // Complex Attribute
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
    public Dictionary<string, Employee> Employees { get; set; }

    // Derivative
    public int NumberOfEmployees => Employees.Count;

    public Restaurant()
    {
        Address = new Address();
        WorkingHours = new Dictionary<DayOfWeek, (TimeOnly, TimeOnly)>();
        Employees = new Dictionary<string, Employee>();
        ObjectStore.Add<Restaurant>(this);
    }


    public Restaurant(Address address) : this()
    {
        Address = address;
    }
}