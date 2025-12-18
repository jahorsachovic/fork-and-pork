namespace fork_and_pork.Classes;

public class FranchisedRestaurant : Restaurant
{
    public float RoyaltyPercent { get; set; }
    public string FranchiseOwner { get; set; }

    public FranchisedRestaurant(float royaltyPercent, string franchiseOwner)
        : base()
    {
        RoyaltyPercent = royaltyPercent;
        FranchiseOwner = franchiseOwner;
        ObjectStore.Add<FranchisedRestaurant>(this);
    }

    public void Delete()
    {
        ObjectStore.Delete(this);
        DeleteRestaurant();
    }
}
