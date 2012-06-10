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
        // name of the wordlist
        const string WordlistFileName = "words.txt";

        // read a wordlist from the directory that the executable is in
        static IEnumerable<string> ReadWordList()
        {
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), WordlistFileName);
            return File.ReadAllLines(file);
        }

        static void Main(string[] args)
        {
            var sw = new Stopwatch();

            // read words
            sw.Start();
            var list = ReadWordList();
            sw.Stop();
            Console.WriteLine("Read {0} words in {1}ms.", list.Count(), sw.ElapsedMilliseconds);
            sw.Reset();

            // build tree
            Console.Write("Building tree... ");
            sw.Start();
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
