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
    public float Calories => _items.Sum(i => i.Calories); //moved here

    private decimal _price;

    [Required]
    [Money]
    [Range(0, int.MaxValue)]
    public decimal Price
    {
        get => _price;
        set
        {
            PropertyValidator.Validate(this, value);
            _price = value;
        }
    }

    //Associations
    private HashSet<MenuItem> _items { get; set; }

    public HashSet<MenuItem> GetMenuItems()
    {
        return new HashSet<MenuItem>(_items);
    }

    public void AddItem(MenuItem item)
    {
        item.AddToComboNoBackRef(this);
        _items.Add(item);
    }

    public void DeleteItem(MenuItem item)
    {
        if (_items.Count == 1) throw new ArgumentException("Combo must have at least one item.");

        _items.Remove(item);
        item.DeleteFromComboNoBackRef(this);
    }


    public Combo(string name, decimal price, HashSet<MenuItem> items)
    {
        if (items.Count == 0)
            throw new ArgumentException("Combo must have at least one item.");
        Name = name;
        Price = price;
        _items = new HashSet<MenuItem>();

        foreach (var item in items)
        {
            AddItem(item);
        }

        ObjectStore.Add(this);
    }
}