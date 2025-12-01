using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class MenuItem
{

    private string _name;

    [Required(ErrorMessage = "MenuItem Name cannot be empty")]
    [MinLength(1)]
    public string Name
    {
        get { return _name;}
        set
        {
            PropertyValidator.Validate(this, value);
            _name = value;
        } 
    }

    private int _calories;

    [Required]
    [Range(0 , int.MaxValue, ErrorMessage = "MenuItem cannot have less than 0 calories.")]
    public int Calories
    {
        get => _calories;
        set
        {
            PropertyValidator.Validate(this, value);
            _calories = value;
        }
    }
    
    
    private decimal _price;

    [Required]
    [Money]
    public decimal Price
    {
        get => _price;
        set
        {
            PropertyValidator.Validate(this, value);
            _price = value;
        }
    }

    public MenuItem(string name, int calories, decimal price)
    {
        Name = name;
        Calories = calories;
        Price = price;
        _combos = new HashSet<Combo>();
        ObjectStore.Add(this);
    }
    
    // Associations
    private HashSet<Combo> _combos;

    public HashSet<Combo> GetCombos()
    {
        return new HashSet<Combo>(_combos);
    }

    public void AddToCombo(Combo combo)
    {
        combo.AddItem(this);
    }
    
    public void AddToComboNoBackRef(Combo combo)
    {
        _combos.Add(combo);
    }

    public void DeleteFromCombo(Combo combo)
    {
        combo.DeleteItem(this);
    }
    
    public void DeleteFromComboNoBackRef(Combo combo)
    {
        _combos.Remove(combo);
    }
    
}