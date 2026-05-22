using System.ComponentModel.DataAnnotations;

namespace inventory.Models;

public class Product
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Product Name is required")]

    public string ProductName { get; set; }

    public string? ProductCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}