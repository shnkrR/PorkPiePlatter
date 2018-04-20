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
                    char[] sep = new char[] { '\r' , '\n' };
                    string[] words = combinedWords.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
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
