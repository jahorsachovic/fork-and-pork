using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class Inspector : Employee
{
    // Attributes
    private uint _licenseId;

    [Required]
    public uint LicenseId
    {
        get => _licenseId;
        set
        {
            PropertyValidator.Validate(this, value);
            _licenseId = value;
        }
    }

    // Associations
    private List<Report> _reports;

    public List<Report> GetReports()
    {
        return new List<Report>(_reports);
    }
    
    public void AddReport(Report report)
    {
        if(report.GetInspector() != this) throw new ArgumentException("Cannot assign report to another inspector.");
        _reports.Add(report);
    }
    
    public void RemoveReport(Report report)
    {
        _reports.Remove(report);
    }
    
    public Inspector(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, decimal salary, Restaurant restaurant, uint licenseId) : base(name, surname, birthDate,
        phoneNumber, email,
        occupation,
        salary, restaurant)
    {
        _reports = new List<Report>();
        LicenseId = licenseId;
        ObjectStore.Add(this);
    }
}