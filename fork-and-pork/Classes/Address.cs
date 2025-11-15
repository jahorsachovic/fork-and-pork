using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class Address
{
    private string _country;
    private string _city;
    private string _street;
    private string _postIndex;
    private string _building;
    
    public Address()
    {
        
    }
    
    public Address(string country, string city, string street, string building, string postIndex): base()
    {
        Country = country;
        City = city;
        Street = street;
        PostIndex = postIndex;
        Building = building;
    }
    
    [Required]
    public string Country
    {
        get { return _country;} 
        set {
            PropertyValidator.Validate(this, value);
            _country = value;
        } 
    }

    [Required]
    public string City {
        get { return _city;} 
        set {
            PropertyValidator.Validate(this, value);
            _city = value;
        } 
    }
    
    [Required]
    public string Street {
        get { return _street;} 
        set {
            PropertyValidator.Validate(this, value);
            _street = value;
        } 
}
    
    [Required]
    public string PostIndex {
        get { return _postIndex;} 
        set {
            PropertyValidator.Validate(this, value);
            _postIndex = value;
        } 
}

    [Required]
    public string Building
    {
        get { return _building; }
        set
        {
            PropertyValidator.Validate(this, value);
            _building = value;
        }
    }

    public override string ToString()
    {
        return $"{Street} {Building}, {City}, {PostIndex}, {Country}";
    }
}