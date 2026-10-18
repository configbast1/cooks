using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

[FirestoreData]
public class Recipe
{
    [FirestoreDocumentId]
    public string RecipeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите название рецепта")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Название: от 3 до 120 символов")]
    [Display(Name = "Название")]
    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите описание")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание: от 10 до 500 символов")]
    [Display(Name = "Краткое описание")]
    [FirestoreProperty]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Перечислите ингредиенты")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "Ингредиенты: от 5 до 2000 символов")]
    [Display(Name = "Ингредиенты")]
    [FirestoreProperty]
    public string Ingredients { get; set; } = string.Empty;

    [Required(ErrorMessage = "Опишите приготовление")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "Приготовление: от 10 до 5000 символов")]
    [Display(Name = "Приготовление")]
    [FirestoreProperty]
    public string Instructions { get; set; } = string.Empty;

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
