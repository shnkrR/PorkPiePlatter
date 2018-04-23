using System.Collections.Generic;
using UnityEngine;


public class LetterDataCollection : ScriptableObject
{
    [System.Serializable]
    public class LetterData
    {
        public string _Letter;
        public int _Count;

        public LetterData()
        {

        }

        public LetterData(string letter, int count)
        {
            _Letter = letter;
            _Count = count;
        }
    }

    public List<LetterData> _Letters = new List<LetterData>();


    public void Initialize()
    {
        for (int  i = 0; i < 26; i++)
        {
            _Letters.Add(new LetterData((char.ConvertFromUtf32(i + 65)).ToString(), 1));
        }
    }
}
