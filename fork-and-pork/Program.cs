using fork_and_pork.Classes;

Console.WriteLine("Hello, world!");

MenuItem mi = new MenuItem("Item1", 200, new decimal(7.99));
//mi.Name = "  ";
mi.Price = new decimal(-8.99);


Console.WriteLine(new Address());
Console.WriteLine($"{mi.Name} + {mi.Calories} + {mi.Price}");
