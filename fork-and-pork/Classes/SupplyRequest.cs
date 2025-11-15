using System.ComponentModel.DataAnnotations;

namespace fork_and_pork.Classes;

public enum RequestStatus
{
    Approved,
    Rejected,
    Pending,
    Completed
}

public class SupplyRequest
{
    private RequestStatus Status { get; set; }

    private DateTime _deadlineDate;

    [Required]
    [FutureDate]
    public DateTime DeadlineDate
    {
        get => _deadlineDate;
        set
        {
            PropertyValidator.Validate(this, value);
            _deadlineDate = value;
        }
    }

    public List<Product> Products { get; set; }

    //   private SupplyRequest(DateTime deadlineDate)
    //   {
    //       Status = RequestStatus.Pending;
    //       DeadlineDate = deadlineDate;
    //       
    //   }
    public SupplyRequest()
    {
        ObjectStore.Add(this);
    }

    public SupplyRequest(DateTime deadlineDate) : this()
    {
        DeadlineDate = deadlineDate;
    }

    public static SupplyRequest RequestSupplies(DateTime deadlineDate)
    {
        //return new SupplyRequest(deadlineDate);
        return new SupplyRequest
        {
            Status = RequestStatus.Pending,
            DeadlineDate = deadlineDate
        };
    }

    public void ChangeStatus(RequestStatus status)
    {
        Status = status;
    }

    public void ReviewRequest(bool isApproved)
    {
        if (isApproved)
        {
            ChangeStatus(RequestStatus.Approved);
        }
        else
        {
            ChangeStatus(RequestStatus.Rejected);
        }
    }
}