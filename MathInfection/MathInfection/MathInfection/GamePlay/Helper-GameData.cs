using System;
using System.Collections.Generic;

namespace MathInfection
{
    public class GameData
    {
        private string playerName;
        private int currentScore;
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
            playerName        = pName;
            currentScore        = 0;
            topScoreCapacity  = 5;
            topScores         = new List<int>(topScoreCapacity);
            topScoresDateTime = new List<DateTime>(topScoreCapacity);
        }
    }
}
