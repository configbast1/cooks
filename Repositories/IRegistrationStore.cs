using Project001.Models;

namespace Project001.Services;

public interface IRegistrationStore
{
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);

    Task<string> SaveAsync(
        RegistrationViewModel model,
        CancellationToken ct);
}
