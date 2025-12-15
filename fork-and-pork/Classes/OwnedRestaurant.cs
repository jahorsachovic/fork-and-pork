namespace fork_and_pork.Classes;

public class OwnedRestaurant : Restaurant
{
    public int AssetsValue { get; set; }

    public OwnedRestaurant(int assetsValue)
        : base()
    {
        AssetsValue = assetsValue;
        ObjectStore.Add<OwnedRestaurant>(this);
    }
}
