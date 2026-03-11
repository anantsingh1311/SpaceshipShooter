using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



///Score Manager Class


namespace CourseWorkPart1
{
    class ScoreManager
    {
        //An event to notify the subscriber when the score is changed 
        public event Action<int> ScoreChanged;
        private int score; // a variable to keep a track of score

        public int Score
        {
            get
            {
                return score;
            }
             set
            {
                if(score!= value)
                {
                    score = value;
                    //ScoreChanged?.Invoke(score);
                    if(ScoreChanged != null)
                    {
                        ScoreChanged.Invoke(score);
                    }
                }
                

            }



        }

        public ScoreManager()
        {
            score = 0;
        }



        //Method to add poinyts
        public void AddToScoreFinal(int points)
        {
            Score += points;
        }

        public void ResetScore()
        {
            score = 0;
        }
        public void AddEnemyKillPoints()
        {
            Score += 50;
            if (Score > 500)
            {
                Score += 50;
            }
        }




    }
}
