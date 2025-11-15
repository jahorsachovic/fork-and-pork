using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class DeliveryRestaurant : Restaurant
{
    private float _deliveryTax;
    
    [Required]
    [Range(0f, 1f, ErrorMessage = "DeliveryTax must be between 0 and 1.")]
    public float DeliveryTax
    {
        get => _deliveryTax;
        set
        {
            PropertyValidator.Validate(this, value);
            _deliveryTax = value;
        }
    }

    private float _deliveryRadius;

    [Required]
    [Range(0, 5, ErrorMessage = "DeliveryRadius must be between 0 and 5.")]
    public float DeliveryRadius
    {
        get => _deliveryRadius;
        set
        {
            PropertyValidator.Validate(this, value);
            _deliveryRadius = value;
        }
    }


    public DeliveryRestaurant(float deliveryTax, float deliveryRadius) : base()
    {
        DeliveryTax = deliveryTax;
        DeliveryRadius = deliveryRadius;
        ObjectStore.Add(this);
    }
}