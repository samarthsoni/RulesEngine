using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    /// <summary>
    /// This program asks the user to type a paragraph and does some calculations based on rules.
    /// </summary>
    class Program
    {
        // Use the following flags to enable/disable the rules
        static bool ruleOneEnabled = true, ruleTwoEnabled = true,
                ruleThreeEnabled = true, ruleFourEnabled = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the text:");
            var text = Console.ReadLine();
            var words = text.Split(' ').ToList();
            ApplyRules(words);

            Console.WriteLine("The output files are saved in working directory");
            Console.Read();
        }

        private static void ApplyRules(IEnumerable<string> words)
        {
            // Rule 1 characters can be reconfigured in the following
            if (ruleOneEnabled)
            {
                var ans = GetRulesConfig("Enter the prefix character in LowerCase for rule 1 if you want to reconfigure");
                var prefixChar = ans != null ? new List<string>() { ans, ans.ToLower() } : new List<string>() { "a", "A" };
                CalcAvgLengthOfWordsStartingWithChar(words, prefixChar);
            }

            // Rule 2 characters can be reconfigured in the following
            if (ruleTwoEnabled)
            {
                var ans = GetRulesConfig("Enter the prefix character in LowerCase for rule 2 if you want to reconfigure");
                var prefixChar = ans != null ? new List<string>() { ans, ans.ToLower() } : new List<string>() { "b", "B" };

                ans = GetRulesConfig("Enter the match character in LowerCase for rule 2 if you want to reconfigure");
                var matchingChar = ans != null ? new List<string>() { ans, ans.ToLower() } : new List<string>() { "e", "E" };
                CountCharInWordsStartingWithChar(words, prefixChar, matchingChar);
            }

            // Rule 3 characters can be reconfigured in the following
            if (ruleThreeEnabled)
            {
                GetLongestWordLengthInWordsStartingWithChar(words, new List<string>() { "a", "b", "c" });
            }

            // Rule 4 characters can be reconfigured in the following
            if (ruleFourEnabled)
            {
                var ans = GetRulesConfig(@"Enter the starting character in LowerCase for first word of rule 4 
                                           if you want to reconfigure");
                var firstChar = ans != null ? new List<string>() { ans, ans.ToLower() } : new List<string>() { "c", "C" };

                ans = GetRulesConfig(@"Enter the starting character in LowerCase for second word of rule 4 
                                       if you want to reconfigure");
                var secondChar = ans != null ? new List<string>() { ans, ans.ToLower() } : new List<string>() { "a", "A" };
                CountConsecutiveWordSequence(words, firstChar, secondChar);
            }
                
            /*
             New rules can be added here.
             */
        }

        private static IEnumerable<string> GetWordsStartingWithCharacter(IEnumerable<string> words, 
            IEnumerable<string> characters)
        {
            var result = words.Where((w) => w.StartsWithAny(characters));
            return result;
        }

        // Rule 1
        private static void CalcAvgLengthOfWordsStartingWithChar(IEnumerable<string> words, 
            IEnumerable<string> startingChar)
        {
            var matchingWords = GetWordsStartingWithCharacter(words, startingChar).ToList();
            var total = 0;
            matchingWords.ForEach((w) =>
            {
                total += w.Length;
            });
            var avg = (float)total / matchingWords.Count();
            WriteToFile(string.Concat("average_length_of_words_starting_with_",startingChar.First(),".txt"), 
                string.Concat("The average is ", avg));
        }

        // Rule 2
        private static void CountCharInWordsStartingWithChar(IEnumerable<string> words, 
            IEnumerable<string> startingChar, IEnumerable<string> matchingChar)
        {
            var matchingWords = GetWordsStartingWithCharacter(words, startingChar).ToList();
            var total = 0;
            matchingWords.ForEach((w) =>
            {
                total += w.Count(c => c == matchingChar.ToList()[0][0] || c == matchingChar.ToList()[1][0]);
            });
            WriteToFile(string.Concat("count_of_",startingChar.First(),"_in_words_starting_with_",matchingChar.First(),".txt"), 
                string.Concat("The total number is ", total));
        }

        // Rule 3
        private static void GetLongestWordLengthInWordsStartingWithChar(IEnumerable<string> words,
            IEnumerable<string> startingChar)
        {
            var matchingWords = GetWordsStartingWithCharacter(words, startingChar).ToList();
            var longestLength = 0;
            matchingWords.ForEach((w) =>
            {
                if (w.Length > longestLength)
                    longestLength = w.Length;
            });
            WriteToFile(string.Concat("longest_words_starting_with_abc.txt"),
                string.Concat("The longest length is ", longestLength));
        }

        //Rule 4
        private static void CountConsecutiveWordSequence(IEnumerable<string> words, IEnumerable<string> startingChar,
            IEnumerable<string> nextWordChar)
        {
            int count = 0;
            for(int i=0; i<words.Count()-1; i++)
            {
                if (words.ToList()[i].StartsWithAny(startingChar))
                {
                    if (words.ToList()[i + 1].StartsWithAny(nextWordChar))
                        count++;
                }
            }
            WriteToFile(string.Concat("count_of_sequence_of_words_starting_with_", startingChar.First(), "_and_"
                , nextWordChar.First(), ".txt"), string.Concat("The total number is ", count));
        }

        private static void WriteToFile(string fileName, string textToWrite)
        {
            File.WriteAllText(fileName, textToWrite);
        }

        private static string GetRulesConfig(string message)
        {
            Console.WriteLine(string.Concat(message, "else press ENTER to use default"));
            var ans = Console.ReadLine();
            return string.IsNullOrEmpty(ans)? null: ans;
        }
    }
}
