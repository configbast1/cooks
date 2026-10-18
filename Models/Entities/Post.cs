using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

[FirestoreData]
public class Post
{
    [FirestoreDocumentId]
    public string PostId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Не указан блог, к которому относится запись")]
    [Display(Name = "Блог")]
    [FirestoreProperty]
    public string BlogId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите заголовок записи")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Заголовок: от 3 до 150 символов")]
    [Display(Name = "Заголовок")]
    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите текст записи")]
    [StringLength(10000, MinimumLength = 10, ErrorMessage = "Текст: от 10 до 10000 символов")]
    [Display(Name = "Текст")]
    [FirestoreProperty]
    public string Content { get; set; } = string.Empty;

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
