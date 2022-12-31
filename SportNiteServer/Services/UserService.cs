using SportNiteServer.Database;
using User = SportNiteServer.Entities.User;

namespace SportNiteServer.Services;

public class UserService
{
    private readonly DatabaseContext _databaseContext;
    
    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<User?> GetUserById(Guid id)
    {
        return await _databaseContext.Users.FindAsync(id);
    }
}