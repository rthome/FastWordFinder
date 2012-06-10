using System;
using System.Collections.Generic;

namespace FastWordFinder
{
    /// <summary>
    /// A container for available and taken characters in a word
    /// </summary>
    sealed class WordCharacters
    {
        readonly List<char> availableChars;
        readonly List<char> takenChars;

        /// <summary>
        /// Try to take a character from the pool of available ones
        /// </summary>
        /// <param name="c">The requested character</param>
        /// <returns>Returns true if the requested character was available, or false if it wasn't</returns>
        public bool Take(char c)
        {
            if (availableChars.Contains(c))
            {
                availableChars.Remove(c);
                takenChars.Add(c);
                return true; 
            }
            return false;
        }

        public override string ToString()
        {
            return new string(takenChars.ToArray());
        }

        /// <summary>
        /// Clone!
        /// </summary>
        /// <returns>Returns a new WordCharacters, with available and taken characters identical to the cloned one</returns>
        public WordCharacters Clone()
        {
            return new WordCharacters(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wchars">The WordCharacters to clone</param>
        private WordCharacters(WordCharacters wchars)
        {
            availableChars = new List<char>(wchars.availableChars);
            takenChars = new List<char>(wchars.takenChars);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chars">The available characters for words</param>
        public WordCharacters(string chars)
        {
            availableChars = new List<char>(chars);
            availableChars.Sort();
            takenChars = new List<char>();
        }
    }
}
