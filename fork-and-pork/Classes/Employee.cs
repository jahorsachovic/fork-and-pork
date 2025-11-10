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
}

public class Employee
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty or just whitespace.");
            }

            _name = value;
        }
    }

    private string _surname;

    public string Surname
    {
        get => _surname;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Surname cannot be empty or just whitespace.");
            }

            _surname = value;
        }
    }

    private DateTime _birthDate;

    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (value > DateTime.Now)
            {
                throw new ArgumentException("BirthDate cannot be in the future.");
            }

            _birthDate = value;
        }
    }

    private DateTime _hireDate;

    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            if (value > DateTime.Now)
            {
                throw new ArgumentException("HireDate cannot be in the future.");
            }

            _hireDate = value;
        }
    }

    // Optional attribute
    private DateTime? _fireDate;

    public DateTime? FireDate
    {
        get => _fireDate;
        private set
        {
            if (value > DateTime.Now)
            {
                throw new ArgumentException("FireDate cannot be in the future.");
            }

            _fireDate = value;
        }
    }

    private string _phoneNumber;

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Phone Number cannot be empty or just whitespace.");
            }

            if (!value.StartsWith('+'))
            {
                throw new FormatException("Phone Number should start with '+' symbol.");
            }
            _phoneNumber = value;
        }
    }

    // Complex attribute
    public string Email { get; set; }

    public Occupation Occupation { get; set; }
    private float _salary;

    public float Salary
    {
        get => _salary;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Salary cannot be less than 0.");
            }

            _salary = value;
        }
    }

    // Associations
    public List<Vacation> Vacations { get; set; }

    public static Employee Add(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, float salary)
    {
        return new Employee
        {
            Name = name,
            Surname = surname,
            BirthDate = birthDate,
            HireDate = DateTime.Now,
            PhoneNumber = phoneNumber,
            Email = email,
            Occupation = occupation,
            Salary = salary
        };
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
        Vacations.Add(new Vacation { FinishDate = finishDate, StartDate = startDate });
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