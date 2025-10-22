﻿using System.Globalization;
using Microsoft.Maui.Storage;
using SortIt.Resources.Localization;

namespace SortIt.Services
{
    public class LanguageService
    {
        public static event Action? LanguageChanged;

        public static void ChangeLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            AppResources.Culture = culture;

            Preferences.Set("AppLanguage", languageCode);
            LanguageChanged?.Invoke();
        }
    }
}
