using Lab5TestTask.Data;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lab5TestTask.Enums;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// UserService implementation.
/// Implement methods here.
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> GetUserAsync()
    {
        return await _dbContext.Users
            .Include(u => u.Sessions)
            .OrderByDescending(u => u.Sessions.Count)
            .Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Status = u.Status,
                Sessions = u.Sessions.Select(s => new Session
                {
                    Id = s.Id,
                    StartedAtUTC = s.StartedAtUTC,
                    EndedAtUTC = s.EndedAtUTC,
                    DeviceType = s.DeviceType,
                    UserId = s.UserId,
                    User = null
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _dbContext.Users
            .Include(u => u.Sessions)
            .Where(u => u.Sessions.Any(s => s.DeviceType == DeviceType.Mobile))
            .Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Status = u.Status,
                Sessions = u.Sessions.Select(s => new Session
                {
                    Id = s.Id,
                    StartedAtUTC = s.StartedAtUTC,
                    EndedAtUTC = s.EndedAtUTC,
                    DeviceType = s.DeviceType,
                    UserId = s.UserId,
                    User = null
                }).ToList()
            })
            .ToListAsync();
    }
}
