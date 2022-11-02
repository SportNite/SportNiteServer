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
            return await _databaseContext.Users
                .Where(x => x.FirebaseUserId == firebaseUserId)
                .Include(x => x.Skills)
                .FirstAsync();
        }

        var user = new User()
        {
            FirebaseUserId = firebaseUserId,
        };
        await _databaseContext.Users.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
        return await _databaseContext.Users.Where(x => x.FirebaseUserId == firebaseUserId)
            .Include(x => x.Skills)
            .FirstAsync();
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

    public async Task<Skill> SetSkill(User user, SetSkillInput input)
    {
        if (await _databaseContext.Skills.Where(x => x.UserId == user.UserId && x.Sport == input.Sport).AnyAsync())
        {
            var existingSkill = await _databaseContext.Skills
                .Where(x => x.UserId == user.UserId && x.Sport == input.Sport)
                .FirstAsync();
            existingSkill.Level = input.Level;
            await _databaseContext.SaveChangesAsync();
            return existingSkill;
        }

        var skill = new Skill
        {
            UserId = user.UserId,
            Sport = input.Sport,
            Level = input.Level
        };
        if (input.SkillId != null) skill.SkillId = input.SkillId.Value;
        await _databaseContext.Skills.AddAsync(skill);
        await _databaseContext.SaveChangesAsync();
        return skill;
    }

    public async Task<Skill> DeleteSkill(User user, Offer.SportType sportType)
    {
        var skill = await _databaseContext.Skills
            .Where(x => x.UserId == user.UserId && x.Sport == sportType)
            .FirstAsync();
        _databaseContext.Skills.Remove(skill);
        await _databaseContext.SaveChangesAsync();
        return skill;
    }
}