namespace SportNiteServer.Entities;

public class Response
{
    public int ResponseId { get; set; }
    public int OfferId { get; set; }
    public int UserId { get; set; }
    public ResponseStatus Status { get; set; }
    public string Description { get; set; }


    public enum ResponseStatus
    {
        Approved,
        Rejected,
        Pending,
        Canceled
    }
}