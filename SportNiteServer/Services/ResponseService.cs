using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;

namespace SportNiteServer.Services;

public class ResponseService
{
    private readonly DatabaseContext _databaseContext;

    public ResponseService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Response> CreateResponse(User user, CreateResponseInput input)
    {
        var response = new Response()
        {
            OfferId = input.OfferId,
            Description = input.Description,
            UserId = user.UserId,
            Status = Response.ResponseStatus.Pending,
        };
        if (input.ResponseId != null) response.ResponseId = input.ResponseId.Value;
        await _databaseContext.Responses.AddAsync(response);
        await _databaseContext.SaveChangesAsync();
        return response;
    }

    public async Task<IEnumerable<Response>> GetMyResponses(User user)
    {
        return _databaseContext.Responses.Where(x => x.UserId == user.UserId).Include(x => x.Offer);
    }
}