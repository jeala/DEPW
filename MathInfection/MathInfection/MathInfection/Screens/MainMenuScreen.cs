﻿using Microsoft.Xna.Framework;

namespace MathInfection
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base("")
        {
            MenuEntry newGameMenuEntry = new MenuEntry("New Game");
            MenuEntry loadGameMenuEntry = new MenuEntry("Load Game");
//            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry highscoreMenuEntry = new MenuEntry("High Scores");
            MenuEntry instructionMenuEntry = new MenuEntry("Controls");
            MenuEntry exitMenuEntry = new MenuEntry("Quit");
            
            newGameMenuEntry.Selected += newGameMenuEntrySelected;
            loadGameMenuEntry.Selected += loadGameMenuEntrySelected;
//            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            highscoreMenuEntry.Selected += highscoreMenuEntrySelected;
            instructionMenuEntry.Selected += instructionMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(newGameMenuEntry);
            MenuEntries.Add(loadGameMenuEntry);
//            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(highscoreMenuEntry);
            MenuEntries.Add(instructionMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void newGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, true,
                               new GameplayScreen(ScreenManager, true));
        }

        void loadGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, false,
                               new GameplayScreen(ScreenManager, false));
        }

//        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
//        {
//            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
//        }

        void highscoreMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighscoreScreen(), e.PlayerIndex);
        }

        void instructionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new InstructionScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Last chance, exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
