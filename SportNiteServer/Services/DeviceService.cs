using Microsoft.EntityFrameworkCore;
using SportNiteServer.Database;
using SportNiteServer.Dto;
using SportNiteServer.Entities;

namespace SportNiteServer.Services;

public class DeviceService
{
    private readonly DatabaseContext _databaseContext;

    public DeviceService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public IEnumerable<Device> GetDevices(User user)
        => _databaseContext.Devices.Where(d => d.UserId == user.UserId);

    public async Task<Device> SetDevice(User user, SetDeviceInput input)
    {
        var device =
            await _databaseContext.Devices.FirstOrDefaultAsync(d =>
                d.DeviceId == input.DeviceId && d.UserId == user.UserId);
        if (device == null)
        {
            device = new Device
            {
                DeviceId = input.DeviceId,
                Token = input.Token,
                UserId = user.UserId
            };
            await _databaseContext.Devices.AddAsync(device);
        }
        else
        {
            device.Token = input.Token;
            _databaseContext.Devices.Update(device);
        }

        await _databaseContext.SaveChangesAsync();
        return device;
    }

    public async Task<Device> DeleteDevice(User user, Guid deviceId)
    {
        var device =
            await _databaseContext.Devices.FirstOrDefaultAsync(d => d.DeviceId == deviceId && d.UserId == user.UserId);
        if (device == null)
            return null;
        _databaseContext.Devices.Remove(device);
        await _databaseContext.SaveChangesAsync();
        return device;
    }
}