using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;

namespace SportNiteServer.Services;

public class AuthService
{
    private readonly DatabaseContext _databaseContext;

    public AuthService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User> GetUser(string? firebaseUserId)
    {
        if (firebaseUserId == null) return null;
        if (await _databaseContext.Users.AnyAsync(x => x.FirebaseUserId == firebaseUserId))
        {
            return await _databaseContext.Users.FirstAsync(x => x.FirebaseUserId == firebaseUserId);
        }

        var user = new User()
        {
            FirebaseUserId = firebaseUserId,
        };
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
        return await _databaseContext.Users.FirstAsync(x => x.FirebaseUserId == firebaseUserId);
    }

    public async Task<User> UpdateUser(User user, UpdateUserInput payload)
    {
        if (payload.Availability != null) user.Availability = payload.Availability;
        if (payload.Bio != null) user.Bio = payload.Bio;
        if (payload.City != null) user.City = payload.City;
        if (payload.Name != null) user.Name = payload.Name;
        if (payload.Sex != null) user.Sex = payload.Sex.Value;
        if (payload.BirthDate != null) user.BirthDate = payload.BirthDate.Value;
        await _databaseContext.SaveChangesAsync();
        return user;
    }
}