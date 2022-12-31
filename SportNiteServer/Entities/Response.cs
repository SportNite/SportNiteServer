using System.ComponentModel.DataAnnotations;
// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618

namespace SportNiteServer.Entities;

public class Response
{
    [Key] public Guid ResponseId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Guid OfferId { get; set; }

    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Guid UserId { get; set; }
    public ResponseStatus Status { get; set; }
    public string Description { get; set; } = "";
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