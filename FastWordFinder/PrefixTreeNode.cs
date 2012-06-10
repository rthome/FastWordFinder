using System.Collections.Generic;
using System.Linq;

namespace FastWordFinder
{
    /// <summary>
    /// Implements a tree node
    /// </summary>
    class PrefixTreeNode
    {
        // mapping of characters to cached nodes
        readonly static Dictionary<char, PrefixTreeNode> canonicalNodes = new Dictionary<char, PrefixTreeNode>();

        /// <summary>
        /// This node's character
        /// </summary>
        public readonly char Character;

        // mapping of characters to child nodes
        protected readonly Dictionary<char, PrefixTreeNode> children;

        /// <summary>
        /// Gets a chached node
        /// </summary>
        /// <param name="c">The character for which to get the cached node</param>
        /// <returns>Returns the cached node</returns>
        public static PrefixTreeNode GetCachedNode(char c)
        {
            PrefixTreeNode result;
            if (!canonicalNodes.TryGetValue(c, out result))
            {
                result = new PrefixTreeNode(c) { IsWord = true };
                canonicalNodes[c] = result;
            }
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether this node is a leaf (no children)
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                return children.Count == 0;
            }
        }

        /// <summary>
        /// Gets or sets whether this node is a a word
        /// </summary>
        public bool IsWord
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of this node's children
        /// </summary>
        public int Count
        {
            get
            {
                return children.Count;
            }
        }

        /// <summary>
        /// Calculates the total size of the (sub-)tree
        /// </summary>
        /// <returns>Returns the total size of the tree</returns>
        public int Size()
        {
            return children.Values.Sum(n => n.Size()) + 1;
        }

        /// <summary>
        /// Inserts a word into the tree
        /// </summary>
        /// <param name="word">The full word</param>
        /// <param name="index">The index to the character the next node will have</param>
        public void InsertWord(string word, int index)
        {
            // if we are at the end of the word, this node is a complete word
            if (word.Length == index + 1)
                IsWord = true;
            else
            {
                // get the next character and check if we have a child with this character
                var next = word[index + 1];
                if (!children.ContainsKey(next))
                {
                    // if we do and if it'll be a leaf, get a cached node
                    if (word.Length == index + 2)
                    {
                        children[next] = GetCachedNode(next);
                    }
                    // else just create a node
                    else
                    {
                        children[next] = new PrefixTreeNode(next);
                    }
                }
                // if we had a node with the next character and if it's a leaf, make it a word instead
                else if (children[next].IsLeaf)
                {
                    children[next] = new PrefixTreeNode(next) { IsWord = true };
                }
                // and finally,  continue inserting
                children[next].InsertWord(word, index + 1);
            }
        }

        /// <summary>
        /// Walk the tree in search of a word
        /// </summary>
        /// <param name="wchars">The WordCharacters</param>
        /// <param name="results">The list into which the results will be added</param>
        public void Walk(WordCharacters wchars, LinkedList<string> results)
        {
            // if we have a char available that matches this node's character
            if (wchars.Take(Character))
            {
                // if this node is a word, take the word so far
                if (IsWord)
                    results.AddLast(wchars.ToString());
                // and continue walking
                foreach(var child in children.Values)
                    child.Walk(wchars.Clone(), results);
            }
        }

        public override string ToString()
        {
            return string.Format("Char: {0}, Children: {1}", Character, Count);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="c">The new node's character</param>
        public PrefixTreeNode(char c)
        {
            children = new Dictionary<char, PrefixTreeNode>();
            Character = c;
        }
    }
}
