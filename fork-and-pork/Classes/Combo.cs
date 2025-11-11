namespace fork_and_pork.Classes;

public class Combo
{
    private string _name;

    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty or just whitespace.");
            }

            _name = value;
        }
    }

    // Derived
    public float Calories => Items.Sum(i => i.Calories); //moved here

    public float Price { get; set; }

    //Associations
    public List<MenuItem> Items { get; set; }

    public Combo(string name, float price)
    {
        Name = name;
        //foreach (MenuItem menuItem in Items)
        //{ }
        Items = new List<MenuItem>();
        // Calories = Items.Sum(i => i.Calories); 
        Price = price;

        ObjectStore.Add(this);
    }
}