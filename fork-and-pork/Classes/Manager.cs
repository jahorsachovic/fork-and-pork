namespace fork_and_pork.Classes;

public class Manager : Employee
{
    public Manager(
        string name, string surname, DateTime birthDate, string phoneNumber, string email, Occupation occupation,
        decimal salary)
        : base(name, surname, birthDate, phoneNumber, email, occupation, salary)
    {
        ObjectStore.Add(this);
    }
}