using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace fork_and_pork.Classes;

public enum Occupation
{
    Chief,
    Cook,
    Waiter,
    Janitor,
    Guard
}

public class Vacation
{
    // Attributes
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }

    public Vacation(DateTime startDate, DateTime finishDate, Employee employee)
    {
        StartDate = startDate;
        FinishDate = finishDate;

        _employee = employee;
        employee.AddVacation(this);

        ObjectStore.Add(this);
    }

    // Associations
    private Employee _employee;

    public Employee Employee
    {
        get => _employee;
    }
}

public class Employee
{
    private string _name;

    public static void DeleteEmployee(Employee emp)
    {
        foreach (var vac in emp._vacations) emp.DeleteVacation(vac);
        ObjectStore.Delete(emp);
    }

    [Required(ErrorMessage = "Name cannot be empty or just whitespace.")]
    public string Name
    {
        get => _name;
        set
        {
            PropertyValidator.Validate(this, value);
            _name = value;
        }
    }

    private string _surname;

    [Required(ErrorMessage = "Surname cannot be empty or just whitespace.")]
    public string Surname
    {
        get => _surname;
        set
        {
            PropertyValidator.Validate(this, value);
            _surname = value;
        }
    }

    private DateTime _birthDate;

    [Required]
    [NotFutureDate(ErrorMessage = "BirthDate cannot be in the future.")]
    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            PropertyValidator.Validate(this, value);
            _birthDate = value;
        }
    }

    private DateTime _hireDate;

    [Required]
    [NotFutureDate(ErrorMessage = "HireDate cannot be in the future.")]
    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            PropertyValidator.Validate(this, value);
            _hireDate = value;
        }
    }

    // Optional attribute
    private DateTime? _fireDate;

    [NotFutureDate(ErrorMessage = "FireDate cannot be in the future.")]
    public DateTime? FireDate
    {
        get => _fireDate;
        private set
        {
            if (value != null)
                PropertyValidator.Validate(this, value);
            _fireDate = value;
        }
    }


    private string _phoneNumber;

    [Required(ErrorMessage = "Phone Number cannot be empty or just whitespace.")]
    [Phone]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            PropertyValidator.Validate(this, value);
            _phoneNumber = value;
        }
    }

    // Complex attribute
    [Required] public MailAddress Email { get; set; }

    public Occupation Occupation { get; set; }

    private decimal _salary;

    [Required]
    [Money]
    [Range(0, int.MaxValue, ErrorMessage = "Salary cannot be less than 0.")]
    public decimal Salary
    {
        get => _salary;
        set
        {
            //if (value < 0)
            //{
            //    throw new ArgumentException("Salary cannot be less than 0.");
            //}
            PropertyValidator.Validate(this, value);
            _salary = value;
        }
    }

    // Associations
    private HashSet<Vacation> _vacations; 

    public HashSet<Vacation> GetVacations()
    {
        return new HashSet<Vacation>(_vacations);
    }

    public void AddVacation(Vacation vacation)
    {
        if (this != vacation.Employee)
            throw new ArgumentException("Vacation belongs to a different Employee.");

        _vacations.Add(vacation);
    }

    public void DeleteVacation(Vacation vacation)
    {
        _vacations.Remove(vacation);
        ObjectStore.Delete(vacation);
    }

    public bool IsOnVacation()
    {
        var match = _vacations.FirstOrDefault(v => v.StartDate < DateTime.Now && v.FinishDate > DateTime.Now);
        return match != null;
    }

    private Employee? _supervisor;

    public Employee? GetSupervisor()
    {
        return _supervisor;
    }

    public void SetSupervisor(Employee supervisor)
    {
        if (supervisor == this)
            throw new ArgumentException("Employee cannot be supervised by themselves.");

        if (_supervisor == supervisor) return;

        if (_supervisor != null)
            DeleteSupervisor();

        _supervisor = supervisor;
        supervisor.AddSupervisedEmployee(this);
    }

    public void DeleteSupervisor()
    {
        if (_supervisor == null)
            throw new NullReferenceException("Employee has no supervisor.");
        _supervisor.DeleteSupervisedEmployee(this);
    }

    private HashSet<Employee> _supervisedEmployees;

    public HashSet<Employee> GetSupervisedEmployees()
    {
        return new HashSet<Employee>(_supervisedEmployees);
    }

    public void AddSupervisedEmployee(Employee emp)
    {
        if (_supervisedEmployees.Contains(emp)) return;
        _supervisedEmployees.Add(emp);
        emp.SetSupervisor(this);
    }

    public void DeleteSupervisedEmployee(Employee emp)
    {
        emp._supervisor = null;
        _supervisedEmployees.Remove(emp);
    }

    private Restaurant? _restaurant;

    public Restaurant? GetRestaurant()
    {
        return _restaurant;
    }

    public void SetRestaurant(Restaurant restaurant)
    {
        if (_restaurant == restaurant) return;
        if(_restaurant != null) RemoveFromRestaurant();
        
        _restaurant = restaurant;
        _restaurant.AddEmployee(this);
    }

    public void RemoveFromRestaurant()
    {
        if (_restaurant == null) return;
        if (_restaurant.GetEmployees().ContainsKey(this.Email.Address))
            _restaurant.RemoveEmployee(this);
        _restaurant = null;
    }


    public Employee(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, decimal salary, Restaurant restaurant = null, Employee? supervisor = null,
        HashSet<Employee>? supervisedEmployees = null)
    {
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Email = new MailAddress(email);
        Occupation = occupation;
        Salary = salary;
        _supervisedEmployees = supervisedEmployees == null
            ? new HashSet<Employee>()
            : new HashSet<Employee>(supervisedEmployees);
        _supervisor = supervisor;
        _vacations = new HashSet<Vacation>();
        SetRestaurant(restaurant);
        ObjectStore.Add<Employee>(this);
    }

    public static Employee Add(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, decimal salary, Restaurant restaurant)
    {
        var emp = new Employee(name, surname, birthDate, phoneNumber, email, occupation, salary, restaurant);
        return emp;
    }

    public void Fire()
    {
        FireDate = DateTime.Now;
    }

    public void Rehire()
    {
        FireDate = null;
        HireDate = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Name} {Surname} {Occupation} \nBirthDate: {BirthDate} \nSalary: {Salary}";
    }
}