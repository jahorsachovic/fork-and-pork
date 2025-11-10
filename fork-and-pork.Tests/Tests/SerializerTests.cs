using System;
using System.Collections.Generic;
using System.Net.Mail;
using NUnit.Framework;

namespace fork_and_pork.Tests.Tests;
using Classes;


public class SerializerTests
{
    [Test]
    public void TestSaveAndLoad()
    {
        Employee e1 = Employee.Add(
            "Joe",
            "Doe", DateTime.Now.AddYears(-35), "+48797777123",
            "joeDoe@gmail.com", Occupation.Chief, 25000);

        Employee e2 = Employee.Add(
            "Alice",
            "Wonderland", DateTime.Now.AddYears(-20), "+48797677123",
            "alice@gmail.com", Occupation.Cook, 12000);

        Restaurant r1 = new Restaurant()
        {
            Employees = new Dictionary<string, Employee>(){ {e1.Email, e1}, {e2.Email, e2}},
            WorkingHours = new Dictionary<DayOfWeek, (TimeOnly, TimeOnly)>()
            {
                {DayOfWeek.Monday, (new TimeOnly(8, 00), new TimeOnly(16,00))},
                {DayOfWeek.Tuesday, (new TimeOnly(8, 00), new TimeOnly(16, 00))},
                {DayOfWeek.Wednesday, (new TimeOnly(8, 00), new TimeOnly(16, 00))},
                {DayOfWeek.Thursday, (new TimeOnly(8, 00), new TimeOnly(16, 00))},
                {DayOfWeek.Friday, (new TimeOnly(8, 00), new TimeOnly(16, 00))},
            },
            Address = new Address(){ 
                Country = "Poland",
                City = "Warsaw",
                Street = "Staszica",
                PostIndex = "03-114",
                Building = "14"
            }
        };
        
        
        ObjectStore.AddAll(e1, e2, r1);
        ObjectStore.Save("test-data.json");

        ObjectStore.Load("test-data.json");
        List<Employee> loadedEmployees = ObjectStore.GetObjectList<Employee>();
        List<Restaurant> loadedRestaurants = ObjectStore.GetObjectList<Restaurant>();


        Assert.That(e1, Is.EqualTo(loadedEmployees[0]));
        Assert.That(e2, Is.EqualTo(loadedEmployees[1]));
        Assert.That(r1, Is.EqualTo(loadedRestaurants[0]));

        loadedEmployees[0].PhoneNumber = "+48797777123";
        Assert.That(loadedEmployees[0], Is.EqualTo(loadedRestaurants[0].Employees[e1.Email]));
        Assert.That(loadedEmployees[0].PhoneNumber, Is.EqualTo(loadedRestaurants[0].Employees[e1.Email].PhoneNumber));
    }
}