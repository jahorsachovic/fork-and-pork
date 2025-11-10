namespace fork_and_pork.Classes;

public class DineInRestaurant : Restaurant
{
    private static float _tipTax = 0.1f;
    public static float TipTax
    {
        get => _tipTax;
        set
        {
           if (value < 0f || value > 1f)
           {
               throw new ArgumentException("TipTax must be between 0 and 1.");
           }

           _tipTax = value;
        }
    }
}