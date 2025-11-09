using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Supplier
{
    private string Name { get; set; }
    private MailAddress Email { get; set; }
    private string Address { get; set; }

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