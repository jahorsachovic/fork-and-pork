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
    public ProductType ProductType { get; set; }

//    private Product(ProductType productType)
//   {
//        ProductType = productType;
//    }
    
    public static Product AddProduct(ProductType productType)
    {
        return new Product
        {
            ProductType = productType
        };
    }
    
}