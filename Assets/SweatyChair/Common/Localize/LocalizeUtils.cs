using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

namespace SweatyChair
{

    public enum Language
    {
        English,
        Spanish,
        French,
        Italian,
        Portuguese,
        German,
        Turkish,
        Russian,
        ChineseTraditional,
        ChineseSimplified,
        Japanese,
        Korean,
        Vietnamese
    }

    public enum TermCategory
    {
        Default,
        Tutorial,
        Menu,
        Game,
        Ending,
        Setting,
        Message,
        Notification,
        Button,
        Loading,
        Shop,
        Leaderboard,
        Achievement,
        Social,
        Card,
        Consumable,
        Avatar,
        Weapon,
        Enemy,
        Rank,
        Cargo,
        Hint,
        Guild,
        Resource,
        World,
        Technology,
        Mission,
        RedeemCode,
        DailyLogin,
        Dungeon,
        Story,
        Intro,
        Inventory,
        Clothing,
        Chest,
    }


    public static class LocalizeUtils
    {

        public static event UnityAction languageChangedEvent;

        public static string systemLanguage
        {
            get
            {
                string tmp = Application.systemLanguage.ToString();
                if (tmp == "ChineseSimplified")
                    tmp = "Chinese (Simplified)";
                if (tmp == "ChineseTraditional")
                    tmp = "Chinese (Traditional)";
                return tmp;
            }
        }

        public static Language currentLanguage
        {
            get
            {
#if UGUI
                string langString = I2.Loc.LocalizationManager.CurrentLanguage;
				if (langString == "Chinese (Simplified)")
					return Language.ChineseSimplified;
				else if (langString == "Chinese (Traditional)")
					return Language.ChineseTraditional;
				if (EnumUtils.IsDefined<Language>(langString))
					return EnumUtils.Parse<Language>(langString);
#endif
                return Language.English;
            }
        }

        public static string currentLanguageCode
        {
            get
            {
#if UGUI
                return I2.Loc.LocalizationManager.CurrentLanguageCode;
#endif
                return "English";
            }
        }

        public static void SetLanguage(Language language)
        {
            SetLanguage(LanguageToString(language));
        }

        public static void SetLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
                return;
#if UGUI
			I2.Loc.LocalizationManager.CurrentLanguage = language;
#endif
            if (languageChangedEvent != null)
                languageChangedEvent();
        }

        public static string LanguageToString(Language language)
        {
            switch (language)
            {
                case Language.ChineseSimplified:
                    return "Chinese (Simplified)";
                case Language.ChineseTraditional:
                    return "Chinese (Traditional)";
                default:
                    return language.ToString();
            }
        }

        public static string GetTerm(TermCategory category, object term)
        {
            if (category == TermCategory.Default)
                return term.ToString();
            return string.Format("{0}/{1}", category, term);
        }

        public static bool HasTerm(TermCategory category, object term)
        {
            return HasTerm(GetTerm(category, term));
        }

        public static bool HasTerm(string categorizedTerm)
        {
#if UGUI
			string dump;
			return I2.Loc.LocalizationManager.TryGetTranslation(categorizedTerm, out dump);
#endif
            return false;
        }

        public static string Get(TermCategory category, object term, Language language)
        {
            if (term == null || string.IsNullOrEmpty(term.ToString()))
                return string.Empty;
            string categorizedTerm = GetTerm(category, term);
            string tmp = Get(categorizedTerm);
            if (tmp == categorizedTerm)
                return Get(term.ToString(), language);
            return tmp;
        }

        public static string Get(TermCategory category, object term)
        {
            if (term == null || string.IsNullOrEmpty(term.ToString()))
                return string.Empty;
            string categorizedTerm = GetTerm(category, term);
            string tmp = Get(categorizedTerm);
            if (tmp == categorizedTerm)
                return Get(term.ToString());
            return tmp;
        }

        public static string Get(string categorizedTerm)
        {
            if (string.IsNullOrEmpty(categorizedTerm))
                return string.Empty;
#if UGUI
			string tmp = I2.Loc.LocalizationManager.GetTranslation(categorizedTerm);
			if (string.IsNullOrEmpty(tmp))
				return categorizedTerm;
			return tmp;
#endif
            return categorizedTerm;
        }

        public static string Get(string categorizedTerm, Language language)
        {
            if (string.IsNullOrEmpty(categorizedTerm))
                return string.Empty;
#if UGUI
			string tmp = I2.Loc.LocalizationManager.GetTranslation(categorizedTerm, overrideLanguage: LanguageToString(language));
			if (string.IsNullOrEmpty(tmp))
				return categorizedTerm;
			return tmp;
#endif
            return categorizedTerm;
        }

        public static string GetFormat(TermCategory category, object term, params object[] args)
        {
            try
            {
                return string.Format(Get(category, term), args);
            }
            catch
            {
                Debug.LogErrorFormat("LocalizeUtils:LocalizeFormat - FormatException at term={0}", Get(category, term));
                return Get(term as string);
            }
        }

        public static string GetFormat(object term, params object[] args)
        {
            return GetFormat(TermCategory.Default, term, args);
        }

        public static string GetPlural(TermCategory category, object term, int num = 2)
        {
            return Get(category, term.ToString().ToPlural(num));
        }

        public static string GetPlural(object term, int num = 2)
        {
            string pluralTerm = term.ToString().ToPlural(num);
            string localizedPluralTerm = Get(pluralTerm);
            if (pluralTerm == localizedPluralTerm) // For languages not having pural, e.g. Chinese
                localizedPluralTerm = Get(term as string);
            return localizedPluralTerm;
        }

        public static void SetLocalizeText(this Text text, TermCategory category, string term)
        {
#if UGUI
            var localize = text.GetComponent<I2.Loc.Localize>();
            if (localize != null)
                localize.Term = GetTerm(category, term);
#endif
        }

        public static string GetSalePercentageString(int percentage)
        {
#if UGUI
			if (I2.Loc.LocalizationManager.CurrentLanguage.Contains("Chinese")) {
				if (percentage % 10 == 0)
					return ((100 - percentage) / 10).ToString();
				else if (percentage > 90)
					return GetFormat("Less than {0}", 1);
				return (100 - percentage).ToString();
			}
#endif
			return percentage.ToString();
		}

	}

}