using System;
using System.Collections.Generic;
using fork_and_pork.Classes;
using NUnit.Framework;

namespace fork_and_pork.Tests.Tests;

public class AssociationsTests
{
    private Employee emp1;
    private Employee emp2;
    private Employee emp3;
    private MenuItem item1;
    private MenuItem item2;
    private Combo combo1;


    [SetUp]
    public void SetUp()
    {
        emp1 = Employee.Add(
            "John",
            "Doe", DateTime.Now.AddYears(-35), "+48797777123",
            "johnDoe@gmail.com", Occupation.Chief, 25000);

        emp2 = Employee.Add(
            "Alice",
            "Wonderland", DateTime.Now.AddYears(-20), "+48797677123",
            "alice@gmail.com", Occupation.Cook, 12000);

        emp3 = new Inspector(
            "Alex",
            "Notwonderland", DateTime.Now.AddYears(-35), "+48123456789",
            "alex@notgmail.com", Occupation.Chief, 25000, 125645645
        );

        item1 = new MenuItem("Pork", 400, new decimal(12.90));
        item2 = new MenuItem("Pasta", 800, new decimal(8.90));


        combo1 = new Combo(
            "Combo1",
            new decimal(12.99),
            new HashSet<MenuItem> { item1 });
    }

    [Test]
    public void TestComboWithNoItemsThrows()
    {
        Exception ex =
            Assert.Throws<ArgumentException>(() => new Combo("Combo", new decimal(12.99), new HashSet<MenuItem>()));
        Assert.That(ex.Message, Is.EqualTo("Combo must have at least one item."));
    }

    [Test]
    public void TestComboAddItem()
    {
        combo1.AddItem(new MenuItem("I2", 300, new decimal(7.99)));
        combo1.AddItem(new MenuItem("I3", 200, new decimal(6.99f)));
        Assert.That(combo1.GetMenuItems().Count == 3);
    }

    [Test]
    public void TestComboDeleteItem()
    {
        combo1.AddItem(item2);
        Assert.That(combo1.GetMenuItems().Count == 2);
        combo1.DeleteItem(item1);
        Assert.That(combo1.GetMenuItems().Count == 1);
        Assert.That(item1.GetCombos().Count == 0);
    }

    [Test]
    public void TestItemDeleteFromComboHasOneItemThrows()
    {
        Assert.That(item1.GetCombos().Count == 1);
        Assert.Throws<ArgumentException>(() => item1.DeleteFromCombo(combo1));
        Assert.That(item1.GetCombos().Count == 1);
        Assert.That(combo1.GetMenuItems().Count == 1);
    }

    [Test]
    public void TestItemDeleteFromCombo()
    {
        combo1.AddItem(item2);
        item1.DeleteFromCombo(combo1);
        Assert.That(item1.GetCombos().Count == 0);
        Assert.That(combo1.GetMenuItems().Count == 1);
    }

    [Test]
    public void TestComboDeleteItemException()
    {
        Combo combo = new Combo("Combo", 2.99m, new HashSet<MenuItem> { item1 });

        Exception ex = Assert.Throws<ArgumentException>(() => combo1.DeleteItem(item1));
        Assert.That(ex.Message, Is.EqualTo("Combo must have at least one item."));
    }

    [Test]
    public void TestAddSameItemToCombo()
    {
        Assert.That(combo1.GetMenuItems().Count == 1);
        combo1.AddItem(item1);
        Assert.That(combo1.GetMenuItems().Count == 1);
    }


    [Test]
    public void TestAddVacation()
    {
        Assert.That(emp1.GetVacations().Count == 0);
        Assert.That(!emp1.IsOnVacation());

        var vacation = new Vacation(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1), emp1);
        Assert.That(emp1.GetVacations().Count == 1);
        Assert.That(emp1.IsOnVacation());
    }

    [Test]
    public void TestDeleteVacation()
    {
        var vacation = new Vacation(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1), emp1);
        emp1.DeleteVacation(vacation);
    }
    
    [Test]
    public void TestAddVacationToTwoEmployeesThrows(){
        var vacation = new Vacation(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1), emp1);
        Exception ex = Assert.Throws<ArgumentException>(() => emp2.AddVacation(vacation));
        Assert.That(ex.Message, Is.EqualTo("Vacation belongs to a different Employee."));
    }

    [Test]
    public void TestEmployeeAddAndSetSupervisor()
    {
        emp2.SetSupervisor(emp1);
        Assert.That(emp2.Supervisor == emp1);

        emp2.SetSupervisor(emp3);
        Assert.That(emp2.Supervisor == emp3);
    }

    [Test]
    public void TestEmployeeAddSelfThrowsArgumentException()
    {
        Exception ex = Assert.Throws<ArgumentException>(() => emp1.AddSupervisedEmployee(emp1));
        Assert.That(ex.Message, Is.EqualTo("Employee cannot be supervised by himself."));
    }

    [Test]
    public void TestEmployeeDeleteSupervisor()
    {
        emp1.AddSupervisedEmployee(emp2);
        emp2.DeleteSupervisor();
        Assert.That(!emp1.GetSupervisedEmployees().Contains(emp2));
        Assert.That(emp2.Supervisor == null);
    }

    [Test]
    public void TestEmployeeDeleteSupervisedEmployee()
    {
        emp1.AddSupervisedEmployee(emp2);
        emp1.DeleteSupervisedEmployee(emp2);
        Assert.That(!emp1.GetSupervisedEmployees().Contains(emp2));
        Assert.That(emp2.Supervisor == null);
    }

    [Test]
    public void TestEmployeeDeleteEmptySuuThrowsNullPointerException()
    {
        Exception ex = Assert.Throws<NullReferenceException>(() => emp1.DeleteSupervisor());
        Assert.That(ex.Message, Is.EqualTo("Employee has no supervisor."));
    }

    
}