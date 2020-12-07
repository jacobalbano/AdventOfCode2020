using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Util
{
    public class StringParser
    {
        public bool HasMaterial => cursor < str.Length;

        public StringParser(string input)
        {
            str = input;
        }

        public int ReadInt()
        {
            if (!TryReadInt(out var result))
                throw new Exception("Failed to find integer in material");

            return result;
        }

        public string ReadUntil(string term, bool skip)
        {
            int start = cursor,
                index = str.IndexOf(term, cursor);
            if (index < 0)
                throw new Exception("Failed to find search term in material");

            if (skip)
                cursor = index + term.Length;
            else
                cursor = index;

            return str[start..index];
        }

        public bool TryReadInt(out int result)
        {
            int start = cursor;
            while (cursor < str.Length)
            {
                if (!char.IsNumber(str[cursor]))
                    break;

                cursor++;
            }

            return int.TryParse(str.AsSpan(start, cursor - start), out result);
        }

        public void Skip(int length)
        {
            cursor += length;
        }

        public void SkipExact(string skip)
        {
            int start = cursor, j = 0;
            while (cursor < str.Length && j < skip.Length)
            {
                if (str[cursor] != skip[j++])
                    throw new Exception("SkipExact encountered a mismatch");

                cursor++;
            }

            if (j < skip.Length - 1)
                throw new Exception("SkipExact expended available material before completing pattern");
        }

        internal void SkipAny(string chars)
        {
            while (cursor < str.Length)
            {
                if (!chars.Any(x => x == str[cursor]))
                    break;
                
                cursor++;
            }
        }

        internal void SkipWhile(Func<char, bool> predicate)
        {
            while (cursor < str.Length && predicate(str[cursor]))
                cursor++;
        }

        public char ReadChar()
        {
            return str[cursor++];
        }

        public string ReadRemainder()
        {
            var result = str.Substring(cursor);
            cursor = str.Length;
            return result;
        }

        private string str;
        private int cursor = 0;
    }
}
