using UnityEngine;
using UnityEngine.UI;

public class PanelLetterObject : UIBase
{
    public Color _PlayerColor;

    public Image _SpriteTile;

    public Text _LabelLetter;


    public void InitializeLetter()
    {
        InitializePanel();

        _SpriteTile.color = _PlayerColor;
    }

    public void EnableLetter(string letter)
    {
        _LabelLetter.text = letter.ToUpper();
    }
}
