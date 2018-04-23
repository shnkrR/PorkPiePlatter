using System.Collections;
using System.Collections.Generic;

public class WordTree
{
    #region Nodes
    /// <summary>
    /// Each node contains an ID (word)
    /// </summary>
    private class WordNode : IEnumerable<WordNode>
    {
        #region Private Members
        /// <summary>
        /// The children this node holds
        /// </summary>
        private Dictionary<string, WordNode> mChildren = new Dictionary<string, WordNode>();
        #endregion

        #region Properties
        /// <summary>
        /// The word this node holds
        /// </summary>
        public string _ID { get; private set; }

        /// <summary>
        /// The parent of this node
        /// </summary>
        public WordNode _Parent { get; private set; }

        /// <summary>
        /// The number of children
        /// </summary>
        public int _ChildCount { get { return mChildren.Count; } }
        #endregion

        /// <summary>
        /// Creates a word node
        /// </summary>
        /// <param name="id">The word this node will hold</param>
        public WordNode(string id)
        {
            _ID = id;
        }

        /// <summary>
        /// Gets a child node from the current node
        /// </summary>
        /// <param name="id">Word to look for in the child nodes</param>
        /// <returns>Node containing the word. If not present returns null</returns>
        public WordNode GetChild(string id)
        {
            // If the node is not present among the children of this node, the function returns null
            if (mChildren.ContainsKey(id))
                return mChildren[id];

            return null;
        }

        /// <summary>
        /// Adds a new node as a child to this one
        /// </summary>
        /// <param name="wordNode">Node to add</param>
        public void Add(WordNode wordNode)
        {
            if (wordNode == null)
                return;

            // Check to see if the node has a parent
            if (wordNode._Parent != null)
            {
                // Node already has a parent. Remove it from there.
                wordNode._Parent.mChildren.Remove(wordNode._ID);
            }

            //Set this node as the parent of the param node and add it to this node's children
            wordNode._Parent = this;
            mChildren.Add(wordNode._ID, wordNode);
        }

        /// <summary>
        /// Debug class to print the contents of a node
        /// </summary>
        public void DebugPrintChildren()
        {
            foreach (KeyValuePair<string, WordNode> pair in mChildren)
                UnityEngine.Debug.Log(pair.Key);
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
    #endregion

    #region Private Members

    /// <summary>
    /// Instance of the word loader class
    /// </summary>
    private WordLoader mWordLoader;

    /// <summary>
    /// Instance of the word tree class
    /// </summary>
    private WordNode mWordTree = new WordNode("Root");

    /// <summary>
    /// Depth of the tree
    /// </summary>
    private int depth = 3;
    #endregion


    /// <summary>
    /// Create a new Word Tree
    /// </summary>
    public WordTree()
    {
        // Init depth
        int depth = 1;

        // Load words
        mWordLoader = new WordLoader();
        string[] words = mWordLoader.LoadWordsFromFile(StringIdentifier.RESOURCE_PATH_WORDS_EN);
        if (words.Length > 0)
        {
            for(int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    //Get the main node that contains the first letter of the word
                    WordNode node = GetNode(words[i].Substring(0, depth));
                    InitTree(ref depth, node, words[i]);
                    depth = 1;
                }
            }
        }
    }

    /// <summary>
    /// Initializes nodes of a tree recursively with every set of letters holding the words 
    /// with those letters as childrens
    /// </summary>
    /// <param name="depthIndex">Current index of the depth of the tree</param>
    /// <param name="node">Node under which future nodes are to be added</param>
    /// <param name="word">Word to add</param>
    private void InitTree(ref int depthIndex, WordNode node, string word)
    {
        // Increase current node depth
        depthIndex++;
        // Are we still under the tree depth length
        if ((depthIndex < depth) && (depthIndex < (word.Length)))
        {
            WordNode newNode = null;
            // Does the parent node contain the substring we are looking for
            if (node.GetChild(word.Substring(0, depthIndex)) == null)
            {
                // If not get/create a child node with the substring and add it to the parent
                newNode = GetNode(word.Substring(0, depthIndex));
                node.Add(newNode);
                InitTree(ref depthIndex, newNode, word);
            }
            else
            {
                // If yes fetch that node and recursively init it once again
                newNode = node.GetChild(word.Substring(0, depthIndex));
                InitTree(ref depthIndex, newNode, word);
            }
        }
        else
        {
            // Add the word to the parent node if the parent node does not have it
            if (node.GetChild(word) == null)
            {
                WordNode newNode = GetNode(word);
                node.Add(newNode);
            }
        }
    }

    /// <summary>
    /// Finds a word in the tree
    /// </summary>
    /// <param name="word">Word to find</param>
    /// <returns>Found word</returns>
    public string FindWord(string word)
    {
        int depth = 1;
        string foundWord = "";
        foundWord = FindWordInNode(ref depth, word, mWordTree);
        return foundWord;
    }

    /// <summary>
    /// Checks to see if a word is present in the tree
    /// </summary>
    /// <param name="word">Word to find</param>
    /// <returns>True, If the word is present, else returns False</returns>
    public bool IsWordPresent(string word)
    {
        int depth = 1;
        string foundWord = "";
        foundWord = FindWordInNode(ref depth, word, mWordTree);
        return (!string.IsNullOrEmpty(foundWord));
    }

    /// <summary>
    /// Recursively checks every node to see if the word is present
    /// </summary>
    /// <param name="depthIndex">Current index of the depth of the tree</param>
    /// <param name="word">Word to find</param>
    /// <param name="node">Parent node to search</param>
    /// <returns>Found word or null</returns>
    private string FindWordInNode(ref int depthIndex, string word, WordNode node)
    {
        string result = "";
        if (!string.IsNullOrEmpty(word))
        {
            // Are we still under the tree depth length
            if ((depthIndex < depth) && (depthIndex < (word.Length)))
            {
                // If yes, Does the parent node contain the sub string of the word required
                WordNode newNode = node.GetChild(word.Substring(0, depthIndex));
                if (newNode != null)
                {
                    // If yes, is it the actual word
                    if (newNode._ID != word)
                    {
                        // If no, increase depth and recursively check for the word again
                        depthIndex++;
                        result = FindWordInNode(ref depthIndex, word, newNode);
                    }
                    else
                    {
                        // If yes, return it
                        return newNode._ID;
                    }
                }
            }
            else
            {
                // If not, does the node contain the word
                WordNode newNode = node.GetChild(word);
                if (newNode != null)
                {
                    // If yes, double check and return
                    if (newNode._ID == word)
                        return newNode._ID;
                }
            }
        }

        // Return the found result
        return result;
    }

    /// <summary>
    /// Prints a tree node
    /// </summary>
    /// <param name="nodeID">Node ID to search and print</param>
    private void DebugPrintTree(string nodeID)
    {
        WordNode node = mWordTree.GetChild(nodeID);
        if (node != null)
        {
            node.DebugPrintChildren();
        }
    }

    /// <summary>
    /// Get a node with the ID. If it does not exist create it.
    /// </summary>
    /// <param name="nodeID">Node ID to get/create</param>
    /// <returns>Node with entered ID</returns>
    private WordNode GetNode(string nodeID)
    {
        WordNode node = mWordTree.GetChild(nodeID);
        if (node == null)
        {
            node = new WordNode(nodeID);
            mWordTree.Add(node);
        }

        return node;
    }
}
