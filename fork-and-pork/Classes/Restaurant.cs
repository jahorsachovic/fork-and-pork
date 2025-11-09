using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Restaurant
{
    public string Address { get; private set; }
    public SortedDictionary<DayOfWeek, (TimeOnly, TimeOnly)> WorkingHours { get; private set; }
    
    // Associations 
    public Dictionary<MailAddress, Employee> Employees { get; private set; }

    // Derivative
    public int NumberOfEmployees => Employees.Count;
    
}