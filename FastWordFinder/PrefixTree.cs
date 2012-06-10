using System.Collections.Generic;

namespace FastWordFinder
{
    /// <summary>
    /// Implements a root node in the tree
    /// </summary>
    sealed class PrefixTree : PrefixTreeNode
    {
        /// <summary>
        /// Build a tree from a list of words
        /// </summary>
        /// <param name="wordlist"></param>
        public void BuildTree(IEnumerable<string> wordlist)
        {
            // for each word in the list
            PrefixTreeNode child;
            foreach (var word in wordlist)
            {
                // if we don't have a child with the first character in the word, create one
                if (!children.TryGetValue(word[0], out child))
                {
                    child = new PrefixTreeNode(word[0]);
                    children.Add(word[0], child);
                }
                
                // insert the word, starting with the child
                child.InsertWord(word, 0);
            }
        }

        /// <summary>
        /// Find a word in the tree
        /// </summary>
        /// <param name="word">The characters from which to find words</param>
        /// <returns>Returns a list of results</returns>
        public IEnumerable<string> FindWords(string word)
        {
            var wchars = new WordCharacters(word);
            var results = new LinkedList<string>();
            foreach (var child in children.Values)
                child.Walk(wchars.Clone(), results);
            return results;
        }

        public PrefixTree()
            : base(' ')
        { }
    }
}
