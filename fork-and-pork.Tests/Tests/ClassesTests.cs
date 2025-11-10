using System;
using System.Collections.Generic;
using System.Net.Mail;
using NUnit.Framework;

namespace fork_and_pork.Tests.Tests;

using Classes;

public class ClassesTests
{
    // Optional
    [Test]
    public void TestEmployee()
    {
        DateTime birthday = DateTime.Now.AddYears(-20);
        Employee e1 = Employee.Add("Jane", "Doe", birthday, "+48999999999", "janedoe@gmail.com", Occupation.Waiter,
            700f);

        Assert.That(e1.Name, Is.EqualTo("Jane"));
        Assert.That(e1.Surname, Is.EqualTo("Doe"));
        Assert.That(e1.BirthDate, Is.EqualTo(birthday));
        Assert.That(e1.PhoneNumber, Is.EqualTo("+48999999999"));
        Assert.That(e1.Email, Is.EqualTo("janedoe@gmail.com"));
        Assert.That(e1.Occupation, Is.EqualTo(Occupation.Waiter));
        Assert.That(e1.Salary, Is.EqualTo(700f));

        Assert.That(e1.FireDate, Is.EqualTo(null));
        e1.Fire();
        Assert.That(e1.FireDate, Is.Not.EqualTo(null));
        e1.Rehire();
        Assert.That(e1.FireDate, Is.EqualTo(null));
    }

    [Test]
    public void TestEmployeeException()
    {
        DateTime birthday = DateTime.Now.AddYears(-20);
        Employee e1 = Employee.Add("Jane", "Doe", birthday, "+48999999999", "janedoe@gmail.com", Occupation.Waiter,
            700);

        Exception ex = Assert.Throws<ArgumentException>(() => e1.Name = "");
        Assert.That(ex.Message, Is.EqualTo("Name cannot be empty or just whitespace."));

        ex = Assert.Throws<ArgumentException>(() => e1.Surname = "");
        Assert.That(ex.Message, Is.EqualTo("Surname cannot be empty or just whitespace."));

        ex = Assert.Throws<ArgumentException>(() => e1.BirthDate = DateTime.Now.AddYears(2000));
        Assert.That(ex.Message, Is.EqualTo("BirthDate cannot be in the future."));

        ex = Assert.Throws<ArgumentException>(() => e1.HireDate = DateTime.Now.AddYears(2000));
        Assert.That(ex.Message, Is.EqualTo("HireDate cannot be in the future."));

        ex = Assert.Throws<ArgumentException>(() => e1.PhoneNumber = "");
        Assert.That(ex.Message, Is.EqualTo("Phone Number cannot be empty or just whitespace."));

        ex = Assert.Throws<FormatException>(() => e1.PhoneNumber = "12345555");
        Assert.That(ex.Message, Is.EqualTo("Phone Number should start with '+' symbol."));

        ex = Assert.Throws<ArgumentException>(() => e1.Salary = -700);
        Assert.That(ex.Message, Is.EqualTo("Salary cannot be less than 0."));
    }

    [Test]
    public void TestMenuItem()
    {
        MenuItem mi = new MenuItem("Item1", 200f, 7.99f);

        Assert.That(mi.Name, Is.EqualTo("Item1"));
        Assert.That(mi.Calories, Is.EqualTo(200f));
        Assert.That(mi.Price, Is.EqualTo(7.99f));
    }

    [Test]
    public void TestMenuItemException()
    {
        MenuItem mi = new MenuItem("Item1", 200, 7.99f);

        Exception ex = Assert.Throws<ArgumentException>(() => mi.Name = " ");
        Assert.That(ex.Message, Is.EqualTo("MenuItem Name cannot be empty or just whitespace."));

        ex = Assert.Throws<ArgumentException>(() => mi.Calories = -200);
        Assert.That(ex.Message, Is.EqualTo("MenuItem cannot have less than 0 calories."));

        ex = Assert.Throws<ArgumentException>(() => mi.Price = -0.99f);
        Assert.That(ex.Message, Is.EqualTo("Price cannot be negative."));
    }
    
    
    [Test]
    public void TestCombo()
    {
        Combo c = new Combo("Combo1", 12.99f);
        c.Items.Add(new MenuItem("I1", 300f, 7.99f));
        c.Items.Add(new MenuItem("I2", 200f, 6.99f));

        Assert.That(c.Name, Is.EqualTo("Combo1"));
        Assert.That(c.Price, Is.EqualTo(12.99f));
        Assert.That(c.Items.Count, Is.EqualTo(2));
        Assert.That(c.Calories, Is.EqualTo(500f).Within(0.1));
    }

    [Test]
    public void TestComboException()
    {
        Combo c = new Combo("Combo1", 12.99f);
        
        Exception ex = Assert.Throws<ArgumentException>(() => c.Name = " ");
        Assert.That(ex.Message, Is.EqualTo("Name cannot be empty or just whitespace."));
    }

    [Test]
    public void TestDineInTaxException()
    {
        Exception ex = Assert.Throws<ArgumentException>(() => DineInRestaurant.TipTax = 1000);
        Assert.That(ex.Message, Is.EqualTo("TipTax must be between 0 and 1."));
    }

    [Test]
    public void TestSupplier()
    {
        Supplier s = Supplier.AddSupplier("Prork", new MailAddress("prork@proton.me"), "King's Road 67");

        Assert.That(s.Name, Is.EqualTo("Prork"));
        Assert.That(s.Email.Address, Is.EqualTo("prork@proton.me"));
        Assert.That(s.Address, Is.EqualTo("King's Road 67"));
    }

    [Test]
    public void TestDineInRestaurant()
    {
        Assert.That(DineInRestaurant.TipTax, Is.EqualTo(0.1f));
        DineInRestaurant.TipTax = 0.2f;
        Assert.That(DineInRestaurant.TipTax, Is.EqualTo(0.2f));
    }

    [Test]
    public void TestDeliveryRestaurant()
    {
        DeliveryRestaurant r = new DeliveryRestaurant(0.2f, 6f);
        Assert.That(r.DeliveryTax, Is.EqualTo(0.2f));
        Assert.That(r.DeliveryRadius, Is.EqualTo(6f));
    }

    // Complex Attribute
    [Test]
    public void TestRestaurant()
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

        Assert.That(r1.NumberOfEmployees == 2);
        Assert.That(e1 == r1.Employees[e1.Email]);
        Assert.That(r1.WorkingHours[DayOfWeek.Monday].Item1, Is.EqualTo(new TimeOnly(8, 00)));
    }
        

}