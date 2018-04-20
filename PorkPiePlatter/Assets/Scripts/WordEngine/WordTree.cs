using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTree
{
    private WordLoader mWordLoader;

    private WordNode mWordTree = new WordNode("Root");

    private int depth = 3;

    //TODO
    //create a function that takes a word in and goes thorugh each layer of the tree and finds the word.
    //look at init tree for help.
    //
    //also refactor init tree
    //

    public WordTree()
    {
        int depth = 1;
        mWordLoader = new WordLoader();
        string[] words = mWordLoader.LoadWordsFromFile(StringIdentifier.RESOURCE_PATH_WORDS_EN);
        if (words.Length > 0)
        {
            for(int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    WordNode node = GetParentNode(words[i].Substring(0, depth));
                    InitTree(ref depth, node, words[i]);
                    depth = 1;
                }
                //if (i == 1)
                //    break;
            }
        }
    }

    private void InitTree(ref int depthIndex, WordNode node, string word)
    {
        depthIndex++;
        if ((depthIndex < depth) && (depthIndex < (word.Length)))
        {
            WordNode newNode = null;
            if (node.GetChild(word.Substring(0, depthIndex)) == null)
            {
                newNode = GetParentNode(word.Substring(0, depthIndex));
                node.Add(newNode);
                InitTree(ref depthIndex, newNode, word);
            }
            else
            {
                newNode = node.GetChild(word.Substring(0, depthIndex));
                node.Add(newNode);
                InitTree(ref depthIndex, newNode, word);
            }
        }
        else
        {
            if (node.GetChild(word) == null)
            {
                WordNode newNode = GetParentNode(word);
                node.Add(newNode);
            }
        }
    }

    public string FindWord(string word)
    {
        int depth = 1;
        string foundWord = "";
        foundWord = FindWordInNode(ref depth, word, mWordTree);
        return foundWord;
    }

    private string FindWordInNode(ref int depthIndex, string word, WordNode node)
    {
        string result = "";
        if (!string.IsNullOrEmpty(word))
        {
            if ((depthIndex < depth) && (depthIndex < (word.Length)))
            {
                WordNode newNode = node.GetChild(word.Substring(0, depthIndex));
                if (newNode != null)
                {
                    if (newNode._ID != word)
                    {
                        depthIndex++;
                        result = FindWordInNode(ref depthIndex, word, newNode);
                    }
                    else
                    {
                        return newNode._ID;
                    }
                }
            }
            else
            {
                WordNode newNode = node.GetChild(word);
                if (newNode != null)
                {
                    if (newNode._ID == word)
                        return newNode._ID;
                }
            }
        }

        return result;
    }

    public void DebugPrintTree(string index)
    {
        WordNode node = mWordTree.GetChild(index);
        if (node != null)
        {
            node.DebugPrintChildren();
        }
    }

    private WordNode GetParentNode(string parentID)
    {
        WordNode node = mWordTree.GetChild(parentID);
        if (node == null)
        {
            node = new WordNode(parentID);
            mWordTree.Add(node);
        }

        return node;
    }
}

public class WordNode : IEnumerable<WordNode>
{
    public string _ID = "";

    private Dictionary<string, WordNode> mChildren = new Dictionary<string, WordNode>();

    public WordNode _Parent { get; private set; }

    public int _ChildCount { get { return mChildren.Count; } }


    public WordNode(string id)
    {
        _ID = id;
    }

    public WordNode GetChild(string id)
    {
        if (mChildren.ContainsKey(id))
            return mChildren[id];

        return null;
    }

    public void Add(WordNode wordNode)
    {
        if (wordNode == null)
            return;

        if (wordNode._Parent != null)
        {
            // Node already has a parent. Remove it from there.
            wordNode._Parent.mChildren.Remove(wordNode._ID);
        }

        wordNode._Parent = this;
        mChildren.Add(wordNode._ID, wordNode);
    }

    public void DebugPrintChildren()
    {
        foreach(KeyValuePair<string, WordNode> pair in mChildren)
            Debug.Log(pair.Key);
    }

    public IEnumerator<WordNode> GetEnumerator()
    {
        return mChildren.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
