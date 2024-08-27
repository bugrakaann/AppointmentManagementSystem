namespace Models.DTOs;

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } // Liste
    public int TotalItems { get; set; } // Toplam öğe sayısı
    public int PageNumber { get; set; } // Geçerli sayfa numarası
    public int PageSize { get; set; } // Sayfa başına öğe sayısı

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize); // Toplam sayfa sayısı
    public bool HasPreviousPage => PageNumber > 1; // Önceki sayfanın olup olmadığını kontrol eder
    public bool HasNextPage => PageNumber < TotalPages; // Sonraki sayfanın olup olmadığını kontrol eder

    public PagedResultDto()
    {
        Items = [];
    }
}