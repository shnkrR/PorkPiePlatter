﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTreeHelper
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

    public WordTreeHelper()
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
        if ((depthIndex < depth) && (depthIndex < (word.Length - 1)))
        {
            WordNode newNode = GetParentNode(word.Substring(0, depthIndex));
            if (node.GetChild(newNode._ID) == null)
            {
                node.Add(newNode);
            }
            InitTree(ref depthIndex, newNode, word);
        }
        else
        {
            WordNode newNode = GetParentNode(word);
            node.Add(newNode);
        }
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