using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Util
{
    public class StringParser
    {
        public StringParser(string input)
        {
            str = input;
        }

        public int ReadInt()
        {
            int start = cursor;
            while (cursor < str.Length)
            {
                var c = str[cursor];
                if (!char.IsNumber(str[cursor]))
                    break;

                cursor++;
            }

            return int.Parse(str.AsSpan(start, cursor - start));
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
