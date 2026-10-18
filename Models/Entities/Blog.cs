using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

[FirestoreData]
public class Blog
{
    [FirestoreDocumentId]
    public string BlogId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите название блога")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название: от 3 до 100 символов")]
    [Display(Name = "Название блога")]
    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите описание")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание: от 10 до 500 символов")]
    [Display(Name = "Описание")]
    [FirestoreProperty]
    public string Description { get; set; } = string.Empty;

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
