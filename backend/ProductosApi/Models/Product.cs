using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
