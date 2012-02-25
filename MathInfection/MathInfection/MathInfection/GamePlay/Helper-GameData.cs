using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChainRxN
{
    public class GameData
    {
        private string playerName;
        private int totalLevel;
        private int totalScore;
        private int lastTotal;
        private int currentLevel;
        private bool lastGameWon;
        private bool middleUpdate;
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

        public int TotalLevel
        {
            get
            {
                return totalLevel;
            }
            set
            {
                totalLevel = value;
            }
        }

        public int TotalScore
        {
            get
            {
                return totalScore;
            }
            set
            {
                totalScore = value;
            }
        }

        public int LastTotal
        {
            get
            {
                return lastTotal;
            }
            set
            {
                lastTotal = value;
            }
        }

        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
            set
            {
                currentLevel = value;
            }
        }

        public bool LastGameWon
        {
            get
            {
                return lastGameWon;
            }
            set
            {
                lastGameWon = value;
            }
        }

        public bool MiddleUpdate
        {
            get
            {
                return middleUpdate;
            }
            set
            {
                middleUpdate = value;
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

        public GameData(string pName, int levels)
        {
            playerName        = pName;
            totalLevel        = levels;
            totalScore        = 0;
            lastTotal         = 0;
            currentLevel      = 1;
            lastGameWon       = false;
            middleUpdate      = false;
            topScoreCapacity  = 5;
            topScores         = new List<int>(topScoreCapacity);
            topScoresDateTime = new List<DateTime>(topScoreCapacity);
        }
    }
}
