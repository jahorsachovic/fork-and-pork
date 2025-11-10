namespace fork_and_pork.Classes;

public class MenuItem
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("MenuItem Name cannot be empty or just whitespace.");
            }

            _name = value;
        }
    }

    private float _calories;

    public float Calories
    {
        get => _calories;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("MenuItem cannot have less than 0 calories.");
            }

            _calories = value;
        }
    }


    private float _price;
    public float Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            _price = value;
        }
    }

    public List<Product> ProductsUsed;

    public MenuItem(string name, float calories, float price)
    {
        Name = name;
        Calories = calories;
        Price = price;
    }
}