using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject obj = new GameObject("GameManager");
                mInstance = obj.AddComponent<GameManager>();
            }

            return mInstance;
        }
    }

    private static GameManager mInstance;

    public System.Action OnPressBack;

    private List<int> mScores;

    private int mSessionScore;

    private const int MAX_SCORES = 10;


    private void Awake()
    {
        mInstance = this;

        mSessionScore = 0;

        mScores = LoadScores();
        if (mScores == null) mScores = new List<int>();
    }

    private void Update()
    {
        // Handle the back key/button functionality
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnPressBack == null)
            {
                // Show quit popup
                Debug.LogError("Quit");
            }
            else
            {
                OnPressBack();
            }
        }
    }

    #region Leaderboards
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
        mScores.Sort((a, b) => -1 * a.CompareTo(b));
        string saveString = "";
        for (int i = 0; i < Mathf.Min(mScores.Count, MAX_SCORES); i++)
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

    public List<int> GetLeaderboard()
    {
        if (mScores != null && mScores.Count > 0)
        {
            mScores.Sort((a, b) => -1 * a.CompareTo(b));
            return mScores;
        }

        return null;
    }
    #endregion
}
