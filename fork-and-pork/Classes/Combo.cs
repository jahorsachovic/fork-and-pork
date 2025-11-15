namespace fork_and_pork.Classes;

using System.ComponentModel.DataAnnotations;


public class Combo
{
    private string _name;
    [Required(ErrorMessage = "Name cannot be empty or just whitespace.")]
    public string Name
    {
        get => _name; 
        set
        {
            PropertyValidator.Validate(this, value);
            _name = value;
        }
    }

    // Derived
    public float Calories => Items.Sum(i => i.Calories); //moved here

    private decimal _price;
    
    [Required]
    [Money]
    [Range(0, int.MaxValue)]
    public decimal Price
    {
        get => _price; 
        set {
            PropertyValidator.Validate(this, value);
            _price = value;
        }
    }

    //Associations
    public List<MenuItem> Items { get; set; }

    public Combo(string name, decimal price)
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