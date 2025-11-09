using System.Net.Mail;
using fork_and_pork.Classes;

Employee e1 = Employee.Add(
"Joe",
"Doe", DateTime.Now.AddYears(-35), "+48797777123",
 new MailAddress("joeDoe@gmail.com"), Occupation.Chief, 25000 );
 
Employee e2 = Employee.Add(
"Alice",
"Wonderland", DateTime.Now.AddYears(-20), "+48797677123",
 new MailAddress("alice@gmail.com"), Occupation.Cook, 12000 );

Console.WriteLine(e1);
Console.WriteLine(e2);

ObjectStore.AddAll(e1, e2);
ObjectStore.Save("data.json");

ObjectStore.Load("data.json");
List<Employee> employees = ObjectStore.GetObjectList<Employee>();

foreach (var employee in employees)
{
 Console.WriteLine(employee);
}

