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
    public float Price { get; private set; }
    //Associations
    public List<MenuItem> Items { get; private set; }

    public Combo(string name, float calories, float price)
    {
        Name = name;
        //foreach (MenuItem menuItem in Items)
        //{ }

        // Calories = Items.Sum(i => i.Calories); 
        Price = price;
    }
   
}