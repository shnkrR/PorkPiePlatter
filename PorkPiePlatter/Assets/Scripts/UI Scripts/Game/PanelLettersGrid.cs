using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelLettersGrid : UIBase
{
    /// <summary>
    /// Grid to spawn letters in
    /// </summary>
    public LayoutGroup _GridLetters;

    /// <summary>
    /// Grid where words are assigned
    /// </summary>
    public LayoutGroup _GridWords;

    /// <summary>
    /// Submit button
    /// </summary>
    public Button _ButtonSubmit;

    /// <summary>
    /// Letter object pool
    /// </summary>
    private List<PanelLetterObject> mObjectLetterPool;

    /// <summary>
    /// Letter base
    /// </summary>
    private LetterBase mLetterBase;

    /// <summary>
    /// Word Tree
    /// </summary>
    private WordTree mWordTree;

    /// <summary>
    /// Size of the pool
    /// </summary>
    private const int LETTER_POOL_SIZE = 9;

    private int mNumLetters = 16;

    private int mChosenLetters = 0;

    private string mChosenWord = "";


    protected override void InitializePanel()
    {
        base.InitializePanel();

        // Inititalizethe word tree
        mWordTree = new WordTree();

        // Initialize the letter object pool
        InitializeLetterObjects();

        // Initialize the letter base
        mLetterBase = new LetterBase();
    }

    protected override void EnablePanel()
    {
        base.EnablePanel();

        SpawnLetters();

        _ButtonSubmit.interactable = false;
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
                    letter.OnLetterUsed += OnLetterPressed;
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
                    letter.OnLetterUsed += OnLetterPressed;
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

        for (int i = 0; i < mObjectLetterPool.Count; i++)
        {
            mObjectLetterPool[i].OnLetterUsed -= OnLetterPressed;
        }
    }

    #region Button Messages
    private void OnLetterPressed(PanelLetterObject letterObject)
    {
        //There's probably a better way to do this. But it's 2 AM in the morning!
        if (letterObject._CachedTransform.parent == _GridLetters.transform)
        {
            // Selected a letter from the list of letters from the bottom
            if (!letterObject._IsHidden)
            {
                // Make sure the letter hasn't already been selected
                // Hide the letter, get a new letter from the pool and set it to the word grid on top
                PanelLetterObject letter = GetLetterObjects(mNumLetters + mChosenLetters);
                letter._CachedTransform.SetParent(_GridWords.transform);
                letter.EnableLetter(letterObject._Letter);
                letter._CachedGameObject.SetActive(true);
                letter._CustomData = letterObject;
                letterObject.ShowLetter(false);
                mChosenLetters++;
                mChosenWord += letterObject._Letter;

                // If the resulting word is a valid word, enable the submit button, otherwise don't
                _ButtonSubmit.interactable = false;
                if (!string.IsNullOrEmpty(mChosenWord))
                {
                    if (mWordTree.IsWordPresent(mChosenWord))
                    {
                        _ButtonSubmit.interactable = true;
                    }
                }
            }
        }
        else
        {
            // Selected a letter from the word on top
            if (letterObject._CachedTransform.parent == _GridWords.transform)
            {
                // Remove the letter from the word grid and send it back to the pool
                // Enable the hidden letter in the letter grid
                PanelLetterObject letter = letterObject._CustomData as PanelLetterObject;
                letter.ShowLetter(true);
                letterObject._CachedGameObject.SetActive(false);
                letterObject._CachedTransform.SetParent(_GridLetters.transform);
                mChosenLetters--;
                mChosenWord = mChosenWord.Replace(letterObject._Letter, "");

                // If the resulting word is a valid word, enable the submit button, otherwise don't
                _ButtonSubmit.interactable = false;
                if (!string.IsNullOrEmpty(mChosenWord))
                {
                    if (mWordTree.IsWordPresent(mChosenWord))
                    {
                        _ButtonSubmit.interactable = true;
                    }
                }
            }
        }
    }

    public void OnWordSubmitted()
    {
        // Get the score of the word adn submit it to the leaderboard
        int score = mWordTree.GetWordScore(mChosenWord);
        GameManager._Instance.SubmitScore(score);

        // Remove the used letters and replace them with new ones
        for (int i = 0; i < mObjectLetterPool.Count; i++)
        {
            if (mObjectLetterPool[i]._CachedTransform.parent == _GridWords.transform)
            {
                // Place the letters in the word grid back into the pool
                mObjectLetterPool[i]._CachedGameObject.SetActive(false);
                mObjectLetterPool[i]._CachedTransform.SetParent(_GridLetters.transform);
            }
            else
            {
                if (mObjectLetterPool[i]._CachedTransform.parent == _GridLetters.transform)
                {
                    if (mObjectLetterPool[i]._IsHidden)
                    {
                        // Change the hidden letters to different letters
                        mObjectLetterPool[i].EnableLetter(mLetterBase.GetLetters(1)[0]);
                        mObjectLetterPool[i].ShowLetter(true);
                    }
                }
            }
        }

        // Reset cached variables
        mChosenLetters = 0;
        mChosenWord = "";
        _ButtonSubmit.interactable = false;
    }
    #endregion
}
