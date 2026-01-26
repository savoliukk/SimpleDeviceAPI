using SimpleDeviceAPI.Models;

namespace SimpleDeviceAPI.Services;

public interface IDeviceService
{
    Task<List<Device>> GetAllDevicesAsync(CancellationToken ct);
    Task<Device?> GetByIdAsync(int id, CancellationToken ct);
    Task AddDeviceAsync(Device device, CancellationToken ct);
}
