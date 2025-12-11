using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace fork_and_pork.Classes;

public class Supplier
{
    private string _name;

    [Required]
    public string Name
    {
        get { return _name; }
        set{
            PropertyValidator.Validate(this, value);
            _name = value;
        }
    }

    public MailAddress Email { get; set; }

    private Address _address;

    [Required]
    public Address Address
    {
        get { return _address; }
        set {
            PropertyValidator.Validate(this, value);
            var context = new ValidationContext(value);
            Validator.ValidateObject(value, context, validateAllProperties: true);
            _address = value;
        }
    }

    private Supplier(string name, MailAddress email, Address address, HashSet<Product> products)
    {
        Name = name;
        Email = email;
        Address = address;
        _products = new HashSet<Product>(products);
        
        //apply reference back to product
        foreach (var product in _products)
        {
            product.AddSupplier(this);
        }
        
        ObjectStore.Add(this);
    }

    public static Supplier AddSupplier(string name, MailAddress email, Address address, HashSet<Product> products)
    {
        return new Supplier(name, email, address, products);
    }

    private HashSet<Product> _products;
    
    public HashSet<Product> GetProductsSupplied()
    {
        return new HashSet<Product>(_products);
    }

    public void AddProductSupplied(Product product)
    {
        //if (product.GetSuppliers().Contains(this)) return;
        if (_products.Contains(product)) return;
        _products.Add(product);
        product.AddSupplier(this);
    }
    
    public void RemoveProduct(Product product){
        if (!_products.Contains(product)) return;
        if (_products.Count == 1) throw new ArgumentException("ProductsSupplied count must be at least one.");

        _products.Remove(product);
        product.RemoveSupplier(this);
    }
    
    
}