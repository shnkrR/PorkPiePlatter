using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelLettersGrid : UIBase
{
    /// <summary>
    /// Grid to spawn letters in
    /// </summary>
    public GridLayoutGroup _GridLetters;

    /// <summary>
    /// Letter object pool
    /// </summary>
    private List<PanelLetterObject> mObjectLetterPool;

    /// <summary>
    /// Letter base
    /// </summary>
    private LetterBase mLetterBase;

    /// <summary>
    /// Size of the pool
    /// </summary>
    private const int LETTER_POOL_SIZE = 9;

    private int mNumLetters = 16;


    protected override void InitializePanel()
    {
        base.InitializePanel();

        //Initialize the letter object pool
        InitializeLetterObjects();

        //Initialize the letter base
        mLetterBase = new LetterBase();
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

    /// <summary>
    /// Spawns letters from the letter set
    /// </summary>
    private void SpawnLetters()
    {
        string[] letters = mLetterBase.GetLetters(mNumLetters);

        // Go through each letter object and intialize it with the chosen letter
        int index;
        for (index = 0; index < letters.Length; index++)
        {
            PanelLetterObject letter = GetLetterObjects(index);
            letter._CachedGameObject.SetActive(true);
            letter.EnableLetter(letters[index]);
        }

        // Turn off the other objects in the pool
        for (int i = index; i < mObjectLetterPool.Count; i++)
        {
            mObjectLetterPool[i]._CachedGameObject.SetActive(false);
        }
    }

    #region Letter Object Pool
    /// <summary>
    /// Initializes the letter object pool
    /// </summary>
    private void InitializeLetterObjects()
    {
        mObjectLetterPool = new List<PanelLetterObject>();

        GameObject objLetter = Resources.Load<GameObject>(StringIdentifier.UI_PREFAB_LETTER_TILE);
        for (int i = 0; i < LETTER_POOL_SIZE; i++)
        {
            if (objLetter != null)
            {
                GameObject objLetterInstance = Instantiate(objLetter) as GameObject;
                PanelLetterObject letter = objLetterInstance.GetComponent<PanelLetterObject>();
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
    
    /// <summary>
    /// Gets a letter object from the pool
    /// </summary>
    /// <param name="index">Index of the object</param>
    /// <returns>Letter object</returns>
    private PanelLetterObject GetLetterObjects(int index)
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
                PanelLetterObject letter = objLetterInstance.GetComponent<PanelLetterObject>();
                if (letter != null)
                {
                    letter.InitializeLetter();
                    mObjectLetterPool.Add(letter);
                    objLetterInstance.transform.SetParent(_GridLetters.transform);
                    objLetterInstance.SetActive(false);
                    objLetterInstance.name = index.ToString("00") + "_Letter";
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
    #endregion

    protected override void DisablePanel()
    {
        base.DisablePanel();
    }

    protected override void DestroyPanel()
    {
        base.DestroyPanel();
    }
}
