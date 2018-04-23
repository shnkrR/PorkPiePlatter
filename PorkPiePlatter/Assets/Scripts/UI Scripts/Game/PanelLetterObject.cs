using UnityEngine;
using UnityEngine.UI;

public class PanelLetterObject : UIBase
{
    public enum eLetterState
    {
        SELECTED,
        DESELECTED,
    }


    public System.Action<PanelLetterObject, PanelLetterObject.eLetterState> OnLetterUsed;

    public Color _PlayerColor;

    public Image _SpriteTile;

    public Text _LabelLetter;

    private eLetterState mLetterState;


    public void InitializeLetter()
    {
        InitializePanel();

        _SpriteTile.color = _PlayerColor;

        mLetterState = eLetterState.DESELECTED;
    }

    public void EnableLetter(string letter)
    {
        _LabelLetter.text = letter.ToUpper();
    }

    #region Button Messages
    public void OnLetterTilePressed()
    {
        mLetterState = (mLetterState == eLetterState.SELECTED) ? eLetterState.DESELECTED : eLetterState.SELECTED;

        if (OnLetterUsed != null)
        {
            OnLetterUsed(this, mLetterState);
        }
    }
    #endregion
}
