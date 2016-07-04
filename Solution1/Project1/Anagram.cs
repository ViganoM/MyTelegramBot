using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task {
    class Anagram {
        AutoResetEvent EndEvent;

        object lockQueue = new object();
        Queue<string> queue = new Queue<string>();
        List<string> result = new List<string>();
        string[] Dictionary;

        int iteration;
        bool finished=false;

        public void StartAnagram(string word, string dictionaryPath, AutoResetEvent endEvent) {
            string[] dictionary;
            if ( !File.Exists(dictionaryPath) ) {
                endEvent.Set();
                return;
            }
            dictionary = File.ReadAllLines(dictionaryPath);
            StartAnagram(word, dictionary, endEvent);

        }
        public void StartAnagram(string word, IEnumerable<string> dictionary, AutoResetEvent endEvent) {
            if ( word.Length <= 1 ) {
                endEvent.Set();
                return;
            }

            EndEvent = endEvent;
            Dictionary = dictionary.Where(x => x.Length == word.Length).ToArray();

            iteration = 0;
            long combinations = Factorial(word.Length);
            string _word = word;

            new Thread(Check).Start();

            do {
                _word = doAnagram(_word);
                lock ( lockQueue )
                    queue.Enqueue(_word);
            } while ( iteration < combinations );

            finished = true;
        }

        string doAnagram(string w) {

            int position = iteration % (w.Length - 1);
            string w2 = w.Remove(position);
            w2 += w.Substring(position + 1, 1);
            w2 += w.Substring(position, 1);
            w2 += w.Substring(position + 2);

            iteration++;
            return w2;
        }

        void Check() {
            do {
                lock ( lockQueue )
                    if ( queue.Count > 0 )
                        if ( !result.Contains(queue.Peek()) && Dictionary.Contains(queue.Peek()) )
                            result.Add(queue.Dequeue());
                        else
                            queue.Dequeue();
                Thread.Sleep(10);

            } while ( queue.Count > 0 || !finished );
            EndEvent.Set();
        }
        public List<string> GetResult() {
            return result;
        }

        long Factorial(int number) {
            if ( number < 1 )
                return 0;
            long factorial = 1;
            for ( int n = 1 ; n <= number ; n++ )
                factorial *= n;
            return factorial;
        }

        /*//other*/

        public static void FindAnagram() {
            string[] dictionary = File.ReadAllLines(myTelegramBot.Properties.Settings.Default.dictionaryPath).ToArray();
            List<string[]> dictionaries = new List<string[]>();
            List<Dictionary<char[], List<string>>> lookup = new List<Dictionary<char[], List<string>>>();
            for ( int n = 0 ; n < 20 ; n++ ) {
                dictionaries.Add(dictionary.Where(x => x.Length == n).ToArray());
                lookup.Add(new Dictionary<char[], List<string>>(new CharComparer()));
            }

            for ( int n = 0 ; n < dictionaries.Count ; n++ ) {
                foreach ( string word in dictionaries[n] ) {
                    Console.WriteLine(word);
                    char[] chars = word.ToCharArray();
                    Array.Sort(chars);
                    if ( lookup[n].ContainsKey(chars) )
                        lookup[n][chars].Add(word);
                    else
                        lookup[n].Add(chars, new List<string>() { word });
                }
            }

            foreach ( Dictionary<char[], List<string>> l in lookup ) {
                List<char[]> indexes = new List<char[]>();
                foreach ( char[] key in l.Keys )
                    if ( l[key].Count < 2 )
                        indexes.Add(key);
                foreach ( char[] key in indexes )
                    l.Remove(key);
            }

            StreamWriter writer = File.AppendText(myTelegramBot.Properties.Settings.Default.writerPath);

            Console.Write("\n---------------------------------------------------------------------\n\n");
            foreach ( Dictionary<char[], List<string>> l in lookup )
                foreach ( char[] key in l.Keys ) {
                    Console.Write("\n--------");
                    writer.Write("\n---------");
                    foreach ( char c in key ) {
                        Console.Write(" " + c);
                        writer.Write("\n---------");
                    }
                    Console.WriteLine("\n");
                    writer.Write("\n\n");
                    foreach ( string word in l[key] ) {
                        Console.WriteLine(word);
                        writer.Write(word+"\n");
                    }
                }
        }

        public class CharComparer : IEqualityComparer<char[]> {
            public bool Equals(char[] x, char[] y) {
                if ( x.Length != y.Length )
                    return false;

                Array.Sort(x);
                Array.Sort(y);

                for ( int n = 0 ; n < x.Length ; n++ )
                    if ( x[n] != y[n] )
                        return false;
                return true;
            }

            public int GetHashCode(char[] obj) {
                return new string(obj).GetHashCode();
            }
        }
    }
}
