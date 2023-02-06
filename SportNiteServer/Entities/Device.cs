namespace SportNiteServer.Entities;

public class Device
{
    public Guid DeviceId { get; set; }
    public string Token { get; set; }
    public Guid UserId { get; set; }
}