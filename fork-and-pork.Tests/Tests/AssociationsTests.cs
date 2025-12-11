using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using fork_and_pork.Classes;
using NUnit.Framework;

namespace fork_and_pork.Tests.Tests;

public class AssociationsTests
{
    private Restaurant r1;
    private Employee emp1;
    private Employee emp2;
    private Inspector emp3; 
    private MenuItem item1;
    private MenuItem item2;
    private Combo combo1;


    [SetUp]
    public void SetUp()
    {
        r1 = new Restaurant
        {
            //changed e1.Email to e1.Email.Address
            //_e = new Dictionary<string, Employee>() { { e1.Email.Address, e1 }, { e2.Email.Address, e2 } },
            Address = new Address("Poland", "Warsaw", "Staszica", "03-114", "14"),
            WorkingHours = new Dictionary<DayOfWeek, (TimeOnly, TimeOnly)>()
            {
                { DayOfWeek.Monday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Tuesday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Wednesday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Thursday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
                { DayOfWeek.Friday, (new TimeOnly(8, 00), new TimeOnly(16, 00)) },
            }
        };


        emp1 = Employee.Add(
            "John",
            "Doe", DateTime.Now.AddYears(-35), "+48797777123",
            "johnDoe@gmail.com", Occupation.Chief, 25000, r1);

        emp2 = Employee.Add(
            "Alice",
            "Wonderland", DateTime.Now.AddYears(-20), "+48797677123",
            "alice@gmail.com", Occupation.Cook, 12000, r1);

        emp3 = new Inspector(
            "Alex",
            "Notwonderland", DateTime.Now.AddYears(-35), "+48123456789",
            "alex@notgmail.com", Occupation.Chief, 25000, r1, 125645645
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
        Assert.That(item1.GetCombos().Contains(combo1));
    }

    [Test]
    public void TestComboDeleteItem()
    {
        combo1.AddItem(item2);
        Assert.That(combo1.GetMenuItems().Count == 2);
        combo1.RemoveItem(item1);
        Assert.That(combo1.GetMenuItems().Count == 1);
        Assert.That(item1.GetCombos().Count == 0);
    }

    [Test]
    public void TestItemDeleteFromComboHasOneItemThrows()
    {
        Console.WriteLine(item1.GetCombos().Count);
        Assert.That(item1.GetCombos().Count == 1);
        Assert.Throws<ArgumentException>(() => item1.RemoveFromCombo(combo1));
        Assert.Throws<ArgumentException>(() => combo1.RemoveItem(item1));
        Assert.That(item1.GetCombos().Count == 1);
        Assert.That(combo1.GetMenuItems().Count == 1);
    }

    [Test]
    public void TestItemDeleteFromCombo()
    {
        combo1.AddItem(item2);
        item1.RemoveFromCombo(combo1);
        Assert.That(item1.GetCombos().Count == 0);
        Console.WriteLine(combo1.GetMenuItems().Count);
        Assert.That(combo1.GetMenuItems().Count == 1);
    }

    [Test]
    public void TestComboDeleteItemException()
    {
        Combo combo = new Combo("Combo", 2.99m, new HashSet<MenuItem> { item1 });

        Exception ex = Assert.Throws<ArgumentException>(() => combo1.RemoveItem(item1));
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

        foreach (Vacation vac in emp1.GetVacations())
        {
            Assert.That(vac.Employee == emp1);
        }
    }

    [Test]
    public void TestDeleteVacation()
    {
        var vacation = new Vacation(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1), emp1);
        emp1.DeleteVacation(vacation);
    }

    [Test]
    public void TestAddVacationToTwoEmployeesThrows()
    {
        var vacation = new Vacation(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1), emp1);
        Exception ex = Assert.Throws<ArgumentException>(() => emp2.AddVacation(vacation));
        Assert.That(ex.Message, Is.EqualTo("Vacation belongs to a different Employee."));
    }

    [Test]
    public void TestEmployeeAddAndSetSupervisor()
    {
        emp2.SetSupervisor(emp1);
        Assert.That(emp2.GetSupervisor() == emp1);
        Assert.That(emp1.GetSupervisedEmployees().Contains(emp2));

        emp2.SetSupervisor(emp3);
        Assert.That(emp2.GetSupervisor() == emp3);
        Assert.That(emp3.GetSupervisedEmployees().Contains(emp2));
        Assert.That(!emp1.GetSupervisedEmployees().Contains(emp2));
    }

    [Test]
    public void TestEmployeeAddSelfThrowsArgumentException()
    {
        Exception ex = Assert.Throws<ArgumentException>(() => emp1.AddSupervisedEmployee(emp1));
        Assert.That(ex.Message, Is.EqualTo("Employee cannot be supervised by themselves."));
    }

    [Test]
    public void TestEmployeeDeleteSupervisor()
    {
        emp1.AddSupervisedEmployee(emp2);
        emp2.DeleteSupervisor();
        Assert.That(!emp1.GetSupervisedEmployees().Contains(emp2));
        Assert.That(emp2.GetSupervisor() == null);
    }

    [Test]
    public void TestEmployeeDeleteSupervisedEmployee()
    {
        emp1.AddSupervisedEmployee(emp2);
        emp1.DeleteSupervisedEmployee(emp2);
        Assert.That(!emp1.GetSupervisedEmployees().Contains(emp2));
        Assert.That(emp2.GetSupervisor() == null);
    }

    [Test]
    public void TestEmployeeDeleteEmptySuuThrowsNullPointerException()
    {
        Exception ex = Assert.Throws<NullReferenceException>(() => emp1.DeleteSupervisor());
        Assert.That(ex.Message, Is.EqualTo("Employee has no supervisor."));
    }

    [Test]
    public void TestProductSupplier()
    {
        Product p1 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s1 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p1});
        
        Assert.That(p1.GetSuppliers().Contains(s1));
        Assert.That(s1.GetProductsSupplied().Contains(p1));
        
        Assert.That(p1.GetSuppliers().Count == 1);
        Assert.That(s1.GetProductsSupplied().Count == 1);
    }
    
    [Test]
    public void TestProductSupplierAddNewSupplierRef()
    {
        Product p1 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s1 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p1});
        
        Product p2 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        p2.AddSupplier(s1);
        
        Assert.That(p2.GetSuppliers().Contains(s1));
        Assert.That(s1.GetProductsSupplied().Contains(p2));
    }
    
    [Test]
    public void TestProductSupplierAddNewProductRef()
    {
        Product p1 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s1 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p1});
        
        Product p2 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s2 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p2});

        Assert.That(p2.GetSuppliers().Contains(s2));
        Assert.That(s2.GetProductsSupplied().Contains(p2));
        
        s1.AddProductSupplied(p2);
        Assert.That(p2.GetSuppliers().Contains(s1));
        Assert.That(s1.GetProductsSupplied().Contains(p2));
    }

    [Test]
    public void TestProductSupplierRemoveProductRef()
    {
        Product p1 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Product p2 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s1 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p1, p2});
        
        s1.RemoveProduct(p2);
        Assert.That(!p2.GetSuppliers().Contains(s1));
        Assert.That(!s1.GetProductsSupplied().Contains(p2));
    }
    
    [Test]
    public void TestProductSupplierRemoveSupplierRef()
    {
        Product p1 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Product p2 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s1 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p1, p2});
        
        p2.RemoveSupplier(s1);
        Assert.That(!p2.GetSuppliers().Contains(s1));
        Assert.That(!s1.GetProductsSupplied().Contains(p2));
    }
    
    [Test]
    public void TestProductSupplierRemoveThrowsException()
    {
        Product p1 = Product.AddProduct(ProductType.Food, new HashSet<Supplier>());
        Supplier s1 = Supplier.AddSupplier("S1", new MailAddress("s1@gmail.com"), new Address(), new HashSet<Product>{p1});
        
        Assert.Throws<ArgumentException>(() => p1.RemoveSupplier(s1));
        Assert.That(p1.GetSuppliers().Contains(s1));
        Assert.That(s1.GetProductsSupplied().Contains(p1));
        
        Assert.Throws<ArgumentException>(() => s1.RemoveProduct(p1));
        Assert.That(p1.GetSuppliers().Contains(s1));
        Assert.That(s1.GetProductsSupplied().Contains(p1));
    }

    [Test]
    public void TestRestaurantEmployeeSetters()
    {
        Restaurant r2 = new Restaurant();
        Assert.That(r1.GetEmployees().Count == 3);
        Assert.That(r2.GetEmployees().Count == 0);
        
        r2.AddEmployee(emp1);
        Assert.That(r2.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() == r2);
        
        emp2.SetRestaurant(r2);
        Assert.That(r2.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() == r2);
        
        Assert.That(r1.GetEmployees().Count == 1);
        Assert.That(r2.GetEmployees().Count == 2);
    }
    
    [Test]
    public void TestRestaurantEmployeeSetNewRestaurant()
    {
        r1.AddEmployee(emp1);
        Assert.That(r1.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() == r1);
        
        Restaurant r2 = new Restaurant();
        emp1.SetRestaurant(r2);
        
        Assert.That(!r1.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() != r1);
        Assert.That(r2.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() == r2);
        
        r1.AddEmployee(emp1);
        Assert.That(!r2.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(r1.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() == r1);
    }
    
    [Test]
    public void TestRestaurantEmployeeRemove()
    {
        r1.RemoveEmployee(emp1);
        Assert.That(!r1.GetEmployees().ContainsKey(emp1.Email.Address));
        Assert.That(emp1.GetRestaurant() != r1);
        Assert.That(emp1.GetRestaurant() == null);
        
        emp2.RemoveFromRestaurant();
        Assert.That(!r1.GetEmployees().ContainsKey(emp2.Email.Address));
        Assert.That(emp2.GetRestaurant() != r1);
        Assert.That(emp2.GetRestaurant() == null);
    }

    [Test]
    public void TestReportCreateAndDelete()
    {
        Report rep1 = Report.SubmitReport(r1, emp3, DateTime.Now.AddDays(-7), DateTime.Now, "Note", Grade.Great);

        Assert.That(r1.GetReports().Contains(rep1));
        Assert.That(emp3.GetReports().Contains(rep1));
        
        rep1.DeleteReport();
        
        Assert.That(!r1.GetReports().Contains(rep1));
        Assert.That(!emp3.GetReports().Contains(rep1));
    }

    [Test]
    public void TestReportAssignToDifferentRestaurantAndInspector()
    {
        Report rep1 = Report.SubmitReport(r1, emp3, DateTime.Now.AddDays(-7), DateTime.Now, "Note", Grade.Great);

        Restaurant r2 = new Restaurant();
        Inspector i2 = new Inspector(
            "Alex",
            "Notwonderland", DateTime.Now.AddYears(-35), "+48123456789",
            "alex@notgmail.com", Occupation.Chief, 25000, r1, 125645645
        );
        
        Assert.Throws<ArgumentException>(() => i2.AddReport(rep1));
        Assert.Throws<ArgumentException>(() => r2.AddReport(rep1));
    }
    
    
}