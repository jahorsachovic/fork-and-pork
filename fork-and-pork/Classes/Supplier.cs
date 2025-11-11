using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Supplier
{
    public string Name { get; set; }
    public MailAddress Email { get; set; }
    public string Address { get; set; }

    public Supplier(string name, MailAddress email, string address)
    {
        Name = name;
        Email = email;
        Address = address;
        ObjectStore.Add(this);
    }

    public static Supplier AddSupplier(string name, MailAddress email, string address)
    {
        return new Supplier(name, email, address);
    }
}