using System.ComponentModel.DataAnnotations;

namespace SimpleDeviceAPI.Contracts;

public sealed class CreateDeviceRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(32)]
    public string? Status { get; set; }
}
