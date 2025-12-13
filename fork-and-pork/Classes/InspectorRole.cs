using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class InspectorRole
{
    // Attributes
    private uint _licenseId;
    private Employee _parent;

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
        if(report.GetInspector() != _parent) throw new ArgumentException("Cannot assign report to another inspector.");
        _reports.Add(report);
    }

    public void RemoveReport(Report report)
    {
        _reports.Remove(report);
    }

    public InspectorRole(Employee parent, uint licenseId)
    {
        _parent = parent;
        _reports = new List<Report>();
        LicenseId = licenseId;
    }
}
