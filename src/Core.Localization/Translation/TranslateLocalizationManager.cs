using System.Data;
using Core.Localization.Abstraction;
using Core.Translation.Abstraction;

namespace Core.Localization.Translation;

public class TranslateLocalizationManager(ITranslationService translationService)
    : ILocalizationService
{
    private const string DefaultLocale = "en";
    public ICollection<string>? AcceptLocales { get; set; }

    private readonly ITranslationService _translationService = translationService;

    public Task<string> GetLocalizedAsync(string key, string? keySection = null)
    {
        return GetLocalizedAsync(
            key,
            AcceptLocales ?? throw new NoNullAllowedException(nameof(AcceptLocales))
        );
    }

    public async Task<string> GetLocalizedAsync(
        string key,
        ICollection<string> acceptLocales,
        string? keySection = null
    )
    {
        string? localization;

        if (acceptLocales is not null)
            foreach (string locale in acceptLocales)
            {
                localization = await _translationService.TranslateAsync(key, locale);
                if (!string.IsNullOrWhiteSpace(localization))
                    return localization;
            }

        localization = await _translationService.TranslateAsync(key, DefaultLocale);
        if (!string.IsNullOrWhiteSpace(localization))
            return localization;

        return key;
    }
}
