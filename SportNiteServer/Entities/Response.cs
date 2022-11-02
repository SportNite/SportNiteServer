using System.ComponentModel.DataAnnotations;

namespace SportNiteServer.Entities;

public class Response
{
    [Key] public Guid ResponseId { get; set; }
    public Guid OfferId { get; set; }
    public Guid UserId { get; set; }
    public ResponseStatus Status { get; set; }
    public string Description { get; set; }
    public Offer Offer { get; set; }
    public User User { get; set; }


    public enum ResponseStatus
    {
        Approved,
        Rejected,
        Pending,
        Canceled
    }
}