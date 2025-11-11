namespace fork_and_pork.Classes;

public class Inspector : Employee
{
    private uint _licenseId;

    public uint LicenseId
    {
        get => _licenseId;
        set
        {
            // if ()
            // TODO
            _licenseId = value;
        }
    }

    public Inspector(string name, string surname, DateTime birthDate, string phoneNumber, string email,
        Occupation occupation, float salary, uint licenseId) : base(name, surname, birthDate, phoneNumber, email,
        occupation,
        salary)
    {
        LicenseId = licenseId;
        ObjectStore.Add(this);
    }
}