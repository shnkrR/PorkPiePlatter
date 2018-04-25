using UnityEngine;
using UnityEngine.UI;

public class PanelLetterObject : UIBase
{
    public System.Action<PanelLetterObject> OnLetterUsed;

    public Color _PlayerColor;

    public Image _SpriteTile;

    public Text _LabelLetter;

    public string _Letter { get { return _LabelLetter.text; } }

    public bool _IsHidden { get { return !_LabelLetter.enabled; } }

    public object _CustomData { get; set; }


    public void InitializeLetter()
    {
        InitializePanel();

        _SpriteTile.color = _PlayerColor;
    }

    public void EnableLetter(string letter)
    {
        _LabelLetter.text = letter.ToUpper();
    }

    public void ShowLetter(bool show)
    {
        _LabelLetter.enabled = show;
        _SpriteTile.enabled = show;
    }

    #region Button Messages
    public void OnLetterTilePressed()
    {
        if (OnLetterUsed != null)
        {
            OnLetterUsed(this);
        }
    }
    #endregion
}
