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
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }

    public Vacation(DateTime startDate, DateTime finishDate)
    {
        StartDate = startDate;
        FinishDate = finishDate;
        ObjectStore.Add(this);
    }
}

public class Employee
{
    private string _name;

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
    [Required]
    public MailAddress Email { get; set; }
    
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
    public List<Vacation> Vacations { get; set; }

    public Employee(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, decimal salary)
    {
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
        Email = new MailAddress(email);
        Occupation = occupation;
        Salary = salary;
        ObjectStore.Add<Employee>(this);
    }

    public static Employee Add(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, decimal salary)
    {
        var emp = new Employee(name, surname, birthDate, phoneNumber, email, occupation, salary);
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

    public void AddVacation(DateTime startDate, DateTime finishDate)
    {
        Vacations.Add(new Vacation(startDate, finishDate));
    }

    public bool IsOnVacation()
    {
        var match = Vacations.FirstOrDefault(v => v.StartDate < DateTime.Now && v.FinishDate > DateTime.Now);
        return match == null;
    }

    public override string ToString()
    {
        return $"{Name} {Surname} {Occupation} \nBirthDate: {BirthDate} \nSalary: {Salary}";
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }
}