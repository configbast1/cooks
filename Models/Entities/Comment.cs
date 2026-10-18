using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

[FirestoreData]
public class Comment
{
    [FirestoreDocumentId]
    public string CommentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Не указана запись, к которой относится комментарий")]
    [FirestoreProperty]
    public string PostId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Представьтесь")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Имя: от 2 до 60 символов")]
    [Display(Name = "Имя")]
    [FirestoreProperty]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Комментарий не может быть пустым")]
    [StringLength(1000, MinimumLength = 2, ErrorMessage = "Комментарий: от 2 до 1000 символов")]
    [Display(Name = "Комментарий")]
    [FirestoreProperty]
    public string Content { get; set; } = string.Empty;

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
