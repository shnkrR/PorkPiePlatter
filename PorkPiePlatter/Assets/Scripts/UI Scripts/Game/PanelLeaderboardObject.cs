using UnityEngine.UI;

public class PanelLeaderboardObject : UIBase
{
    /// <summary>
    /// Rank of the entry
    /// </summary>
    public Text _LabelRank;

    /// <summary>
    /// Name of the entry
    /// </summary>
    public Text _LabelName;

    /// <summary>
    /// Score
    /// </summary>
    public Text _LabelScore;


    public void InitializeLeaderboardEntry()
    {
        InitializePanel();

        _LabelRank.text = "";
        _LabelName.text = "";
        _LabelScore.text = "";
    }

    public void EnableLeaderboardObject(int rank, string name, int score)
    {
        _LabelRank.text = rank.ToString("00");
        _LabelName.text = name;
        _LabelScore.text = score.ToString("00");
    }
}
