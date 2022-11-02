using SportNiteServer.Entities;

namespace SportNiteServer.Dto;

public class CreateResponseInput
{
    public Guid? ResponseId { get; set; }
    public Guid OfferId { get; set; }
    public string Description { get; set; }
}