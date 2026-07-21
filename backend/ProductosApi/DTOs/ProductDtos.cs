using System.ComponentModel.DataAnnotations;

namespace ProductosApi.DTOs;

/// <summary>Representación de un producto que se devuelve al cliente.</summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>Datos requeridos para crear o editar un producto.</summary>
public class ProductInputDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }
}
