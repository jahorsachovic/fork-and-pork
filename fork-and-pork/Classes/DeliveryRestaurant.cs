namespace fork_and_pork.Classes;

public class DeliveryRestaurant : Restaurant
{
    private float _deliveryTax;

    public float DeliveryTax
    {
        get => _deliveryTax;
        set
        {
            if (value < 0f || value > 1f)
            {
                throw new ArgumentException("DeliveryTax must be between 0 and 1.");
            }

            _deliveryTax = value;
        }
    }

    private float _deliveryRadius;

    public float DeliveryRadius
    {
        get => _deliveryRadius;
        set
        {
            if (value <= 0f || value > 10000f)
            {
                throw new ArgumentException("DeliveryRadius must be between 0 and 10000.");
            }

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