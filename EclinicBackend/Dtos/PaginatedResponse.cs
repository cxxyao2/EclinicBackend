using System.ComponentModel.DataAnnotations;
namespace EclinicBackend.Dtos;
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();

    [Required]
    public int CurrentPage { get; set; }
    [Required]
    public int PageSize { get; set; }
    [Required]
    public int TotalItems { get; set; }
    [Required]
    public int TotalPages { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}