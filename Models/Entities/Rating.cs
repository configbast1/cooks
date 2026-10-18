using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

[FirestoreData]
public class Rating
{
    [FirestoreDocumentId]
    public string RatingId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Не указан рецепт")]
    [FirestoreProperty]
    public string RecipeId { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Не указан пользователь")]
    [FirestoreProperty]
    public int UserId { get; set; }

    [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
    [Display(Name = "Оценка")]
    [FirestoreProperty]
    public int Value { get; set; }

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
