using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Supplier
{
    public string Name { get; set; }
    public MailAddress Email { get; set; }
    public string Address { get; set; }

    public static Supplier AddSupplier(string name, MailAddress email, string address)
    {
        return new Supplier
        {
            Name = name,
            Email = email,
            Address = address

        };
    }

}