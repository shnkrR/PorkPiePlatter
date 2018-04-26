using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance { get; private set; }

    private List<int> mScores;

    private int mSessionScore;


    private void Awake()
    {
        _Instance = this;

        mSessionScore = 0;

        mScores = LoadScores();
        if (mScores == null) mScores = new List<int>();
    }

    private List<int> LoadScores()
    {
        string scoreString = PlayerPrefs.GetString(StringIdentifier.PREF_SCORE, "");
        if (!string.IsNullOrEmpty(scoreString))
        {
            string[] splitScores = scoreString.Split('|');
            if (splitScores != null && splitScores.Length > 0)
            {
                List<int> scores = new List<int>();
                for (int i = 0; i < splitScores.Length; i++)
                {
                    if (!string.IsNullOrEmpty(splitScores[i]))
                    {
                        int score = int.Parse(splitScores[i]);
                        if (score > 0)
                        {
                            scores.Add(score);
                        }
                    }
                }
                return scores;
            }
        }

        return null;
    }

    private void SaveScores()
    {
        string saveString = "";
        for (int i = 0; i < mScores.Count; i++)
        {
            saveString += "" + mScores[i] + "|";
        }
        PlayerPrefs.SetString(StringIdentifier.PREF_SCORE, saveString);
    }

    public void SubmitScore(int score)
    {
        if (score > 0)
        {
            if (mSessionScore <= 0)
            {
                mScores.Add(0);
            }

            mSessionScore += score;
            mScores[mScores.Count - 1] = mSessionScore;
            SaveScores();
        }
    }
}
