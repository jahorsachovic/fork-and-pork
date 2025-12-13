namespace fork_and_pork.Classes;

public class RestaurantInspector
{
    private Restaurant _restaurant;
    private Employee _employee;
    private List<Report> _reports;

    public RestaurantInspector(Restaurant restaurant, Employee employee)
    {
        _restaurant = restaurant;
        _employee = employee;
        _reports = new List<Report>();

    }
}
