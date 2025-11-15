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

    public Product(ProductType productType)
    {
        ProductType = productType;
        ObjectStore.Add(this);
    }


    public static Product AddProduct(ProductType productType)
    {
        return new Product(productType);
    }
}