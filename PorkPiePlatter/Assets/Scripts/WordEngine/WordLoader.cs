using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLoader
{
    public string[] LoadWordsFromFile(string a_FilePath)
    {
        if (!string.IsNullOrEmpty(a_FilePath))
        {
            TextAsset asset = Resources.Load<TextAsset>(a_FilePath);
            if (asset != null)
            {
                string combinedWords = asset.text;
                if (!string.IsNullOrEmpty(combinedWords))
                {
                    string[] words = combinedWords.Split('\n');
                    if (words.Length > 0)
                    {
                        return words;
                    }
                }
            }
        }

        return null;
    }
}
