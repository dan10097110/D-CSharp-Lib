using System;
using System.Linq;

namespace DLib
{
    public static class Linguistics
    {
        public static char[] GetChar(string s) => s.
               Where(c => c != ' ').
               ToArray();

        public static char[] GetLetter(string s) => s.
            Where(c => letters.Contains(c)).ToArray();

        public static string[] GetWords(string s) => s.
            Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
            AsParallel().
            Select(t => new string(t.Where(c => letters.Contains(c)).ToArray())).
            Where(t => !string.IsNullOrWhiteSpace(t)).
            ToArray();

        public static string[] GetSentences(string s) => s.
            Replace("Mr.", "Mr").
            Split(sentenceEnding.Select(c => c.ToString()).ToArray(), StringSplitOptions.RemoveEmptyEntries);

        public static string[] GetSubSentences(string s) => s.
            Replace("Mr.", "Mr").
            Split(sentenceEnding.Union(sentenceDividing).Select(c => c.ToString()).ToArray(), StringSplitOptions.RemoveEmptyEntries);


        public static double GetXPerY(int xCount, int yCount) => xCount / (double)yCount;

        public static double GetCharPerSentence(string s) => GetChar(s).Length / (double)GetSentences(s).Length;

        public static double GetCharPerSentence2(string s) => AverageItemLength(GetSentences(s));

        public static double GetLetterPerSentence(string s) => GetLetter(s).Length / (double)GetSentences(s).Length;

        public static double GetWordsPerSentence(string s) => GetWords(s).Length / (double)GetSentences(s).Length;

        public static double GetCharPerSubSentence(string s) => GetChar(s).Length / (double)GetSubSentences(s).Length;

        public static double GetLetterPerSubSentence(string s) => GetLetter(s).Length / (double)GetSubSentences(s).Length;

        public static double GetWordsPerSubSentence(string s) => GetWords(s).Length / (double)GetSubSentences(s).Length;

        public static double GetSubSentencePerSentence(string s) => GetSubSentences(s).Length / (double)GetSentences(s).Length;

        public static double GetLetterPerWord(string s) => AverageItemLength(GetWords(s));

        public static double GetLetterPerWord2(string s) => GetLetter(s).Length / (double)GetWords(s).Length;


        public static (char charr, double share)[] CharShare(string s) => ItemShare(GetChar(s));

        public static (char letter, double share)[] LetterShare(string s) => ItemShare(GetLetter(s));

        public static (string word, double share)[] WordShare(string s) => ItemShare(GetWords(s));


        public static double AverageItemLength(string[] s) => s.Sum(t => t.Length) / (double)s.Length;

        public static (T item, double share)[] ItemShare<T>(T[] t) => t.
            Distinct().
            Select((a, i) => (a, t.Count(b => b.Equals(a)) / (double)t.Length)).
            OrderByDescending(b => b.Item2).
            ToArray();


        public static char[]
            letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ä', 'ö', 'ü', 'ß', 'é', 'á' },
            numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' },
            brackets = new char[] { '(', ')', '[', ']', '{', '}' },
            quotation = new char[] { '\'', '\"', '„', '“', '”' },
            sentenceEnding = new char[] { '.', '?', '!' },
            sentenceDividing = new char[] { ':', ',', ';', '-', '–', '—' };
    }
}
