using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FastWordFinder
{
    class Program
    {
        // name of the fallback wordlist
        const string WordlistFileName = "words.txt";

        static IEnumerable<string> ReadWordList(string filename)
        {
            if (!File.Exists(filename))
                throw new IOException("Specified word list does not exist: " + filename);

            string line;
            using (var sr = new StreamReader(filename))
                while ((line = sr.ReadLine()) != null)
                    yield return line;
        }

        static void Main(string[] args)
        {
            // get the correct word list file
            string wordlistPath;
            if (args.Length > 0)
                wordlistPath = args[0];
            else
                wordlistPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), WordlistFileName);

            var sw = new Stopwatch();
            
            Console.Write("Building tree... ");
            sw.Start();

            var list = ReadWordList(wordlistPath);
            var tree = new PrefixTree();
            tree.BuildTree(list);

            sw.Stop();
            Console.WriteLine("done in {0}ms.", sw.ElapsedMilliseconds);
            sw.Reset();

            // find words
            while (true)
            {
                Console.Write("Characters> ");
                var chars = Console.ReadLine();

                // find a list of words
                sw.Start();
                var results = tree.FindWords(chars);
                sw.Stop();
                Console.WriteLine("Found {0} words in {1}ms", results.Count(), sw.ElapsedMilliseconds);
                sw.Reset();

                // sort result to get the longest words, take as much as we can show
                var sortedResults = results.OrderByDescending(n => n.Length).Take(Console.WindowHeight - 3);
                foreach (var word in sortedResults)
                    Console.WriteLine("{0} ({1})", word.PadRight(sortedResults.First().Length), word.Length);
            }
        }
    }
}
