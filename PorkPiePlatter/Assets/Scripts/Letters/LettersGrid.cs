using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LettersGrid : UIBase
{
    public GridLayoutGroup _GridLetters;

    private List<LetterObject> mObjectLetterPool;

    private const int LETTER_POOL_SIZE = 9;

    private string[] mLetterSet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I",
                                                 "J", "K", "L", "M", "N", "O", "P", "Q", "R",
                                                 "S", "T", "U", "V", "X", "Y", "Z"};


    protected override void InitializePanel()
    {
        base.InitializePanel();

        InitializeLetterObjects();
    }

    protected override void EnablePanel()
    {
        base.EnablePanel();

        SpawnLetters();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnLetters();
    }

    private void SpawnLetters()
    {
        string[] letters = GetLetterSet(9);
        int index;
        for (index = 0; index < letters.Length; index++)
        {
            LetterObject letter = GetLetterObjects(index);
            letter._CachedGameObject.SetActive(true);
            letter.EnableLetter(letters[index]);
        }

        for (int i = index; i < mObjectLetterPool.Count; i++)
        {
            mObjectLetterPool[i]._CachedGameObject.SetActive(false);
        }
    }

    private string[] GetLetterSet(int count)
    {
        if (mLetterSet != null && mLetterSet.Length > 0 && mLetterSet.Length > count)
        {
            mLetterSet.Shuffle();

            string[] result = new string[count];
            int added = 0;
            for (int i = 0; i < mLetterSet.Length; i++)
            {
                float probability = ((float)added / (mLetterSet.Length - added));
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

    private void InitializeLetterObjects()
    {
        mObjectLetterPool = new List<LetterObject>();

        GameObject objLetter = Resources.Load<GameObject>(StringIdentifier.UI_PREFAB_LETTER_TILE);
        for (int i = 0; i < LETTER_POOL_SIZE; i++)
        {
            if (objLetter != null)
            {
                GameObject objLetterInstance = Instantiate(objLetter) as GameObject;
                LetterObject letter = objLetterInstance.GetComponent<LetterObject>();
                if (letter != null)
                {
                    letter.InitializeLetter();
                    mObjectLetterPool.Add(letter);
                    objLetterInstance.transform.SetParent(_GridLetters.transform);
                    objLetterInstance.SetActive(false);
                    objLetterInstance.name = i.ToString("00") + "_Letter";
                }
                else
                {
                    Destroy(objLetterInstance);
                }
            }
        }
        objLetter = null;
    }

    private LetterObject GetLetterObjects(int index)
    {
        if (index < mObjectLetterPool.Count)
        {
            return mObjectLetterPool[index];
        }
        else
        {
            GameObject objLetter = Resources.Load<GameObject>(StringIdentifier.UI_PREFAB_LETTER_TILE);
            if (objLetter != null)
            {
                GameObject objLetterInstance = Instantiate(objLetter) as GameObject;
                LetterObject letter = objLetterInstance.GetComponent<LetterObject>();
                if (letter != null)
                {
                    letter.InitializeLetter();
                    mObjectLetterPool.Add(letter);
                    objLetterInstance.name = index.ToString("00") + "_Letter";
                    objLetterInstance.SetActive(false);
                    return letter;
                }
                else
                {
                    Destroy(objLetterInstance);
                }
            }
            objLetter = null;
        }

        return null;
    }

    protected override void DisablePanel()
    {
        base.DisablePanel();
    }

    protected override void DestroyPanel()
    {
        base.DestroyPanel();
    }
}
