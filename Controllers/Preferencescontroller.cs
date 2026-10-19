using Project001.Services;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Project001.Controllers;

/// <summary>Отдельная страница: https://localhost:7114/Preferences</summary>
public class PreferencesController : Controller
{
    private readonly IRegistrationStore _store;
    private readonly FirestoreDb _db;
    private readonly ILogger<PreferencesController> _logger;

    public PreferencesController(IRegistrationStore store, FirestoreDb db, ILogger<PreferencesController> logger)
    {
        _store = store;
        _db = db;
        _logger = logger;
    }

    // GET /Preferences
    [HttpGet]
    public IActionResult Index()
        => View("Index", new CookingPreferencesViewModel());

    [HttpPost("Preferences")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(CookingPreferencesViewModel model, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(model.CookingLevel) && !CookingPreferencesViewModel.Levels.Contains(model.CookingLevel))
            ModelState.AddModelError(nameof(model.CookingLevel), "Выберите значение из списка");
        if (!string.IsNullOrEmpty(model.Frequency) && !CookingPreferencesViewModel.Frequencies.Contains(model.Frequency))
            ModelState.AddModelError(nameof(model.Frequency), "Выберите значение из списка");
        if (!string.IsNullOrEmpty(model.Diet) && !CookingPreferencesViewModel.Diets.Contains(model.Diet))
            ModelState.AddModelError(nameof(model.Diet), "Выберите значение из списка");
        if (!string.IsNullOrEmpty(model.CookingTime) && !CookingPreferencesViewModel.Times.Contains(model.CookingTime))
            ModelState.AddModelError(nameof(model.CookingTime), "Выберите значение из списка");

        // Допускаем только известные варианты из чипов
        model.Interests = model.Interests.Where(CookingPreferencesViewModel.InterestOptions.Contains).Distinct().ToList();
        model.Equipment = model.Equipment.Where(CookingPreferencesViewModel.EquipmentOptions.Contains).Distinct().ToList();

        if (!ModelState.IsValid)
            return View("Index", model);

        try
        {
            // Анкета привязывается к уже зарегистрированному email
            if (!await _store.EmailExistsAsync(model.Email, ct))
            {
                ModelState.AddModelError(nameof(model.Email), "Этот email не найден. Сначала пройдите регистрацию.");
                return View("Index", model);
            }

            var data = new Dictionary<string, object>
            {
                ["email"] = model.Email.Trim().ToLowerInvariant(),
                ["cookingLevel"] = model.CookingLevel ?? "",
                ["frequency"] = model.Frequency ?? "",
                ["cookingTime"] = model.CookingTime ?? "",
                ["diet"] = model.Diet ?? "",
                ["allergies"] = model.Allergies?.Trim() ?? "",
                ["interests"] = model.Interests,
                ["equipment"] = model.Equipment,
                ["about"] = model.About?.Trim() ?? "",
                ["createdAt"] = DateTime.UtcNow
            };

            var reference = await _db.Collection("cooking_preferences").AddAsync(data, ct);
            _logger.LogInformation("Cooking preferences saved: {Id}", reference.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось сохранить кулинарную анкету в Firestore");
            ModelState.AddModelError(string.Empty, "Не удалось сохранить данные. Попробуйте ещё раз позже.");
            return View("Index", model);
        }

        TempData["PrefsSaved"] = true;
        return RedirectToAction(nameof(Index));
    }
}

/// <summary>Данные отдельной формы с вопросами про готовку.</summary>
public class CookingPreferencesViewModel
{
    public static readonly string[] Levels = { "Новичок", "Любитель", "Опытный", "Профессионал" };
    public static readonly string[] Frequencies = { "Каждый день", "Несколько раз в неделю", "Раз в неделю", "Пару раз в месяц", "Редко" };
    public static readonly string[] Times = { "До 20 минут", "20–40 минут", "40–60 минут", "Больше часа" };
    public static readonly string[] Diets = { "Без ограничений", "Вегетарианская", "Веганская", "Безглютеновая", "Безлактозная", "Кето", "Другая" };
    public static readonly string[] InterestOptions =
        { "Выпечка и десерты", "Мясные блюда", "Рыба и морепродукты", "Вегетарианская кухня", "Супы", "Салаты и закуски", "Азиатская кухня", "Напитки" };
    public static readonly string[] EquipmentOptions =
        { "Духовка", "Мультиварка", "Аэрогриль", "Блендер", "Миксер", "Гриль", "Слоу-кукер", "Соковыжималка" };

    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    [StringLength(120)]
    public string Email { get; set; } = "";

    public string? CookingLevel { get; set; }
    public string? Frequency { get; set; }
    public string? CookingTime { get; set; }
    public string? Diet { get; set; }
        
    [StringLength(150, ErrorMessage = "Максимум 150 символов")]
    public string? Allergies { get; set; }

    public List<string> Interests { get; set; } = new();
    public List<string> Equipment { get; set; } = new();

    [StringLength(300, ErrorMessage = "Максимум 300 символов")]
    public string? About { get; set; }
}