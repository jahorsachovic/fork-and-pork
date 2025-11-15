using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class Inspector : Employee
{
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

    public Inspector(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, decimal salary, uint licenseId) : base(name, surname, birthDate, phoneNumber, email,
        occupation,
        salary)
    {
        LicenseId = licenseId;
        ObjectStore.Add(this);
    }
}