using Microsoft.Xna.Framework;

namespace MathInfection
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry rateGameMenuEntry;
        MenuEntry languageMenuEntry;

        enum Rating
        {
            Perfect,
            Superb,
        }

        static Rating currentRating = Rating.Perfect;

        static string[] todoList = { "We", "need", "options!" };
        static int currentTodoList = 0;

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
            languageMenuEntry.Text = "What's Next? : " + todoList[currentTodoList];
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
            currentTodoList = (currentTodoList + 1) % todoList.Length;

            SetMenuEntryText();
        }
    }
}
