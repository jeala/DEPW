using Microsoft.Xna.Framework;

namespace ChainRxN
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry rateGameMenuEntry;
        MenuEntry languageMenuEntry;

        enum Rating
        {
            Great,
            Perfect,
            Superb,
        }

        static Rating currentRating = Rating.Great;

        static string[] languages = { "C#", "C# again", "C# only :D" };
        static int currentLanguage = 0;

        public OptionsMenuScreen() : base("Options")
        {
            rateGameMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            rateGameMenuEntry.Selected += RateGameMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            back.Selected += OnCancel;
            
            MenuEntries.Add(rateGameMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(back);
        }


        void SetMenuEntryText()
        {
            rateGameMenuEntry.Text = "Rate this Game: " + currentRating;
            languageMenuEntry.Text = "Language Used: " + languages[currentLanguage];
        }

        void RateGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentRating++;

            if (currentRating > Rating.Superb)
                currentRating = 0;

            SetMenuEntryText();
        }

        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }
    }
}
