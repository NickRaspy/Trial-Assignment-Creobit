using UnityEngine.UI;

namespace CB_TA.Clicker
{
    public class Score
    {
        private Text text;
        private int scoreAmount;
        public int ScoreAmount
        {
            get { return scoreAmount; }
            set
            {
                scoreAmount = value;
                text.text = scoreAmount.ToString();
            }
        }

        public Score(Text text)
        {
            this.text = text;
        }

        public void Add() => ScoreAmount++;
    }
}