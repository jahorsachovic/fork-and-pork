namespace fork_and_pork.Classes;

using System.ComponentModel.DataAnnotations;

public class DineInService
{
    private static float _tipTax = 0.1f;

    public static float TipTax
    {
        get => _tipTax;
        set
        {
            if (value < 0f || value > 1f)
            {
                throw new ValidationException("TipTax must be between 0 and 1.");
            }
            _tipTax = value;
        }
    }

    public DineInService()
    {
    }
}
