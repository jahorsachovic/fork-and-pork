using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public class DeliveryService
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

    public DeliveryService(float deliveryTax, float deliveryRadius)
    {
        DeliveryTax = deliveryTax;
        DeliveryRadius = deliveryRadius;
    }

    public void Delete()
    {
        ObjectStore.Delete(this);
    }
}
