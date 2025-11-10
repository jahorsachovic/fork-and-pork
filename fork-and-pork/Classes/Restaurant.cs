using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Address {
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostIndex { get; set; }
    public string Building { get; set; }
}

public class Restaurant
{
    public Address Address { get; set; }

    // Complex Attribute
    public Dictionary<DayOfWeek, (TimeOnly, TimeOnly)> WorkingHours { get; set; }
    
    // Associations 
    public Dictionary<string, Employee> Employees { get; set; }

    // Derivative
    public int NumberOfEmployees => Employees.Count;
    
}