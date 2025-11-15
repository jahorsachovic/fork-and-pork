using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Supplier
{
    private string _name;

    [Required]
    public string Name
    {
        get { return _name; }
        set{
            PropertyValidator.Validate(this, value);
            _name = value;
        }
    }

    public MailAddress Email { get; set; }

    private Address _address;

    [Required]
    public Address Address
    {
        get { return _address; }
        set {
            PropertyValidator.Validate(this, value);
            var context = new ValidationContext(value);
            Validator.ValidateObject(value, context, validateAllProperties: true);
            _address = value;
        }
    }

    public Supplier(string name, MailAddress email, Address address)
    {
        Name = name;
        Email = email;
        Address = address;
        ObjectStore.Add(this);
    }

    public static Supplier AddSupplier(string name, MailAddress email, Address address)
    {
        return new Supplier(name, email, address);
    }
}