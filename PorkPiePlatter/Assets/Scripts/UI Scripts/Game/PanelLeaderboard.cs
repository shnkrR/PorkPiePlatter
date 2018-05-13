using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLeaderboard : UIBase
{
    public LayoutGroup _GridLeaderboard;

    public GameObject _ObjGame;

    private List<PanelLeaderboardObject> mObjectLeaderboardPool;

    private const int LEADERBOARD_POOL_SIZE = 10; 


    protected override void InitializePanel()
    {
        base.InitializePanel();

        InitializeLetterObjects();
    }

    #region Leaderboard Object Pool
    private void InitializeLetterObjects()
    {
        mObjectLeaderboardPool = new List<PanelLeaderboardObject>();

        GameObject objLeaderboard = Resources.Load<GameObject>(StringIdentifier.UI_PREFAB_LEADERBOARD_OBJECT);
        if (objLeaderboard != null)
        {
            for (int i = 0; i < LEADERBOARD_POOL_SIZE; i++)
            {
                GetLeaderboardObject(i);
            }
        }
        objLeaderboard = null;
    }

    public PanelLeaderboardObject GetLeaderboardObject(int index)
    {
        if (index < mObjectLeaderboardPool.Count)
        {
            return mObjectLeaderboardPool[index];
        }
        else
        {
            GameObject objLeaderboard = Resources.Load<GameObject>(StringIdentifier.UI_PREFAB_LEADERBOARD_OBJECT);
            if (objLeaderboard != null)
            {
                GameObject objLeaderboardInstance = Instantiate(objLeaderboard) as GameObject;
                PanelLeaderboardObject leaderboardObject = objLeaderboardInstance.GetComponent<PanelLeaderboardObject>();
                if (leaderboardObject != null)
                {
                    leaderboardObject.InitializeLeaderboardEntry();
                    leaderboardObject._CachedGameObject.SetActive(false);
                    mObjectLeaderboardPool.Add(leaderboardObject);
                    objLeaderboardInstance.transform.SetParent(_GridLeaderboard.transform);
                    objLeaderboardInstance.name = index.ToString("00") + "_LeaderboardObject";
                }
                else
                {
                    Destroy(objLeaderboardInstance);
                    objLeaderboardInstance = null;
                }
            }
            objLeaderboard = null;
        }

        return null;
    }
    #endregion

    public void Enable()
    {
        EnablePanel();

        GameManager._Instance.OnPressBack += Disable;

        DisplayLeaderboard();
    }

    private void DisplayLeaderboard()
    {
        int[] scores = GameManager._Instance.GetLeaderboard().ToArray();
        int index;
        for (index = 0; index < scores.Length; index++)
        {
            if (scores[index] > 0)
            {
                PanelLeaderboardObject leaderboardObject = GetLeaderboardObject(index);
                if (leaderboardObject != null)
                {
                    leaderboardObject.EnableLeaderboardObject(index + 1, "Player", scores[index]);
                    leaderboardObject._CachedGameObject.SetActive(true);
                }
            }
        }

        // Turn off the other objects in the pool
        for (int i = index; i < mObjectLeaderboardPool.Count; i++)
        {
            mObjectLeaderboardPool[i]._CachedGameObject.SetActive(false);
        }
    }

    public void Disable()
    {
        GameManager._Instance.OnPressBack -= Disable;

        _ObjGame.SetActive(true);

        base.DisablePanel();
    }

    protected override void DestroyPanel()
    {
        GameManager._Instance.OnPressBack -= Disable;

        base.DestroyPanel();
    }
}
