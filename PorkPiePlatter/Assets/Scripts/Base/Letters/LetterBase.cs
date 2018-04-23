using System.Collections.Generic;
using UnityEngine;

public class LetterBase
{
    /// <summary>
    /// Letters available to spawn
    /// </summary>
    private List<string> mLetterSet = new List<string>();


    public LetterBase()
    {
        // Load list of letters from the scriptable object
        LetterDataCollection letterDataCollection = Resources.Load<LetterDataCollection>(StringIdentifier.RESOURCE_PATH_LETTER_COLLECTION);
        if (letterDataCollection != null)
        {
            for (int i = 0; i < letterDataCollection._Letters.Count; i++)
            {
                for (int j = 0; j < letterDataCollection._Letters[i]._Count; j++)
                {
                    mLetterSet.Add(letterDataCollection._Letters[i]._Letter);
                }
            }

            Resources.UnloadAsset(letterDataCollection);
            letterDataCollection = null;

            mLetterSet.Shuffle();
        }
    }

    /// <summary>
    /// Get a set of letters
    /// </summary>
    /// <param name="count">Number of letters to spawn</param>
    /// <returns>string array containing letters</returns>
    public string[] GetLetters(int count)
    {
        // Right now this shuffle the letter array, then goes through every single
        // letter and uses a probability of (required/remaining) to pick the letters. Shuffle is
        // probably enough. Need to look at this later.
        if (mLetterSet != null && mLetterSet.Count > 0 && mLetterSet.Count > count)
        {
            mLetterSet.Shuffle();

            string[] result = new string[count];
            int added = 0;
            for (int i = 0; i < mLetterSet.Count; i++)
            {
                float probability = ((float)added / (mLetterSet.Count - added));
                float random = Random.Range(0f, 1f);
                if (probability <= random)
                {
                    if (((added > 0) && (result[added - 1] != mLetterSet[i])) || (added <= 0))
                    {
                        result[added] = mLetterSet[i];
                        added++;

                        if (added >= count)
                            return result;
                    }
                }
            }
        }

        return null;
    }
}
