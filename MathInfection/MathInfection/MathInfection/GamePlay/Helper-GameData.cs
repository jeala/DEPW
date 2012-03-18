using System;
using System.Collections.Generic;

namespace MathInfection
{
    public class GameData
    {
        private string playerName;
        private int currentScore;
        private int currentHealth;
        private bool lastGameDied;
        private int topScoreCapacity;
        private List<int> topScores;
        private List<DateTime> topScoresDateTime;

        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
            }
        }

        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentScore = value;
            }
        }

        public int CurrentHealth
        {
            set
            {
                currentHealth = value;
            }
            get
            {
                return currentHealth;
            }
        }

        public bool LastGameDied
        {
            get
            {
                return lastGameDied;
            }
            set
            {
                lastGameDied = value;
            }
        }

        public int TopScoreCapacity
        {
            set
            {
                topScoreCapacity = value;
            }
            get
            {
                return topScoreCapacity;
            }
        }

        public List<int> TopScores
        {
            get
            {
                return topScores;
            }
            set
            {
                topScores = value;
            }
        }

        public List<DateTime> TopScoresDateTime
        {
            get
            {
                return topScoresDateTime;
            }
            set
            {
                topScoresDateTime = value;
            }
        }

        public GameData()
        {
            
        }

        public GameData(string pName)
        {
            playerName          = pName;
            currentScore        = 0;
            currentHealth       = 100;
            lastGameDied        = false;
            topScoreCapacity    = 5;
            topScores           = new List<int>(topScoreCapacity);
            topScoresDateTime   = new List<DateTime>(topScoreCapacity);
        }
    }
}
