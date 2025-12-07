using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public enum ProductType
{
    Food,
    Utensils,
    Uniform,
    Furniture,
    Machinery
}

public class Product
{
    [Required]
    public ProductType ProductType { get; set; }
    
    private Product(ProductType productType, HashSet<Supplier>? suppliers)
    {
        if (suppliers == null || suppliers.Count < 1) throw new ArgumentException("Product should have at least one supplier.");
        ProductType = productType;
        _suppliers = new HashSet<Supplier>(suppliers);
        ObjectStore.Add(this);
    }    

    public static Product AddProduct(ProductType productType, HashSet<Supplier> suppliers)
    {
        return new Product(productType, suppliers);
    }   

    private HashSet<Supplier> _suppliers;

    public HashSet<Supplier> GetSuppliers()
    {
        return new HashSet<Supplier>(_suppliers);
    }
    
    public void AddSupplier(Supplier supplier)
    {
        if (supplier.GetProductsSupplied().Contains(this)) return;
        _suppliers.Add(supplier);
        supplier.AddProductSupplied(this);
    }
    
    public void RemoveSupplier(Supplier supplier)
    {
        if (!_suppliers.Contains(supplier)) return;        
       
        _suppliers.Remove(supplier);
        supplier.RemoveProduct(this);
    }
    
    public void ReplaceSupplier(Supplier s1, Supplier s2)
    {
        s1.RemoveProduct(this);    
        s2.AddProductSupplied(this);
    }
}