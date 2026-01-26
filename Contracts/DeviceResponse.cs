namespace SimpleDeviceAPI.Contracts;

public sealed class DeviceResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Unknown";
}
