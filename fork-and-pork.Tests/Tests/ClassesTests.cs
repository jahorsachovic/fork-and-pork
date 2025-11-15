using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            700m);

        Assert.That(e1.Name, Is.EqualTo("Jane"));
        Assert.That(e1.Surname, Is.EqualTo("Doe"));
        Assert.That(e1.BirthDate, Is.EqualTo(birthday));
        Assert.That(e1.PhoneNumber, Is.EqualTo("+48999999999"));
        Assert.That(e1.Email.Address, Is.EqualTo("janedoe@gmail.com"));
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
            700m);

        Exception ex = Assert.Throws<ValidationException>(() => e1.Name = "");
        Assert.That(ex.Message, Is.EqualTo("Name cannot be empty or just whitespace."));

        ex = Assert.Throws<ValidationException>(() => e1.Surname = "");
        Assert.That(ex.Message, Is.EqualTo("Surname cannot be empty or just whitespace."));

        ex = Assert.Throws<ValidationException>(() => e1.BirthDate = DateTime.Now.AddYears(2000));
        Assert.That(ex.Message, Is.EqualTo("BirthDate cannot be in the future."));

        ex = Assert.Throws<ValidationException>(() => e1.HireDate = DateTime.Now.AddYears(2000));
        Assert.That(ex.Message, Is.EqualTo("HireDate cannot be in the future."));

        ex = Assert.Throws<ValidationException>(() => e1.PhoneNumber = "");
        Assert.That(ex.Message, Is.EqualTo("Phone Number cannot be empty or just whitespace."));

        ex = Assert.Throws<ValidationException>(() => e1.PhoneNumber = "123a");
        Assert.That(ex.Message, Is.EqualTo("The PhoneNumber field is not a valid phone number."));

        ex = Assert.Throws<ValidationException>(() => e1.Salary = -700);
        Assert.That(ex.Message, Is.EqualTo("The price must have only 2 decimal places and be positive."));
    }

    [Test]
    public void TestMenuItem()
    {
        MenuItem mi = new MenuItem("Item1", 200, new decimal(7.99));

        Assert.That(mi.Name, Is.EqualTo("Item1"));
        Assert.That(mi.Calories, Is.EqualTo(200f));
        Assert.That(mi.Price, Is.EqualTo(7.99f));
    }

    [Test]
    public void TestMenuItemException()
    {
        MenuItem mi = new MenuItem("Item1", 200, new decimal(7.99));

        Exception ex = Assert.Throws<ValidationException>(() => mi.Name = " ");
        Assert.That(ex.Message, Is.EqualTo("MenuItem Name cannot be empty"));
        
        ex = Assert.Throws<ValidationException>(() => mi.Calories = -200);
        Assert.That(ex.Message, Is.EqualTo("MenuItem cannot have less than 0 calories."));
        
        ex = Assert.Throws<ValidationException>(() => mi.Price = new decimal(-0.99));
        Assert.That(ex.Message, Is.EqualTo("The price must have only 2 decimal places and be positive."));
    }


    [Test]
    public void TestCombo()
    {
        Combo c = new Combo("Combo1", new decimal(12.99));
        c.Items.Add(new MenuItem("I1", 300, new decimal(7.99)));
        c.Items.Add(new MenuItem("I2", 200, new decimal(6.99f)));

        Assert.That(c.Name, Is.EqualTo("Combo1"));
        Assert.That(c.Price, Is.EqualTo(12.99f));
        Assert.That(c.Items.Count, Is.EqualTo(2));
        Assert.That(c.Calories, Is.EqualTo(500f).Within(0.1));
    }

    [Test]
    public void TestComboException()
    {
        Combo c = new Combo("Combo1", new decimal(12.99f));

        Exception ex = Assert.Throws<ValidationException>(() => c.Name = " ");
        Assert.That(ex.Message, Is.EqualTo("Name cannot be empty or just whitespace."));
    }

    [Test]
    public void TestDineInTaxException()
    {
        Exception ex = Assert.Throws<ValidationException>(() => DineInRestaurant.TipTax = 1000);
        Assert.That(ex.Message, Is.EqualTo("TipTax must be between 0 and 1."));
    }

    [Test]
    public void TestSupplier()
    {
        Supplier s = Supplier.AddSupplier("Prork", new MailAddress("prork@proton.me"), new Address("Country", "City", "Str", "8", "00000"));

        Assert.That(s.Name, Is.EqualTo("Prork"));
        Assert.That(s.Email.Address, Is.EqualTo("prork@proton.me"));
        Assert.That(s.Address.ToString(), Is.EqualTo(new Address("Country", "City", "Str", "8", "00000").ToString()));
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
        DeliveryRestaurant r = new DeliveryRestaurant(0.2f, 4f);
        Assert.That(r.DeliveryTax, Is.EqualTo(0.2f));
        Assert.That(r.DeliveryRadius, Is.EqualTo(4f));
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

        Restaurant r1 = new Restaurant
        {
            //changed e1.Email to e1.Email.Address
            Employees = new Dictionary<string, Employee>() { { e1.Email.Address, e1 }, { e2.Email.Address, e2 } },
            WorkingHours = new Dictionary<DayOfWeek, (TimeOnly, TimeOnly)>()
            {
                { DayOfWeek.Monday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Tuesday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Wednesday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Thursday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Friday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
            },
            Address = new Address("Poland", "Warsaw", "Staszica", "03-114", "14"),
        };

        Assert.That(r1.NumberOfEmployees == 2);
        Assert.That(e1 == r1.Employees[e1.Email.Address]); //same here
        Assert.That(r1.WorkingHours[DayOfWeek.Monday].Item1, Is.EqualTo(new TimeOnly(8, 00)));
    }
}