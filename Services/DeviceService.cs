using Microsoft.EntityFrameworkCore;
using SimpleDeviceAPI.Data;
using SimpleDeviceAPI.Models;

namespace SimpleDeviceAPI.Services;

public sealed class DeviceService : IDeviceService
{
    private readonly AppDbContext _db;

    public DeviceService(AppDbContext db) => _db = db;

    public async Task<List<Device>> GetAllDevicesAsync(CancellationToken ct)
        => await _db.Devices.AsNoTracking().ToListAsync(ct);

    public async Task<Device?> GetByIdAsync(int id, CancellationToken ct)
        => await _db.Devices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddDeviceAsync(Device device, CancellationToken ct)
    {
        _db.Devices.Add(device);
        await _db.SaveChangesAsync(ct);
    }
}
