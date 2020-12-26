using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common
{
    public class Grid<T>
    {
        public int Rows { get; }
        public int Columns { get; }

        public T this[int row, int col]
        {
            get => storage[row, col];
            set => storage[row, col] = value;
        }

        public IEnumerable<(int row, int col)> Cells()
        {
            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Columns; col++)
                    yield return (row, col);
        }

        public Grid(string input, Func<char, T> parser)
        {
            var lines = input
                .ToLines()
                .ToArray();

            Rows = lines.Length;
            Columns = lines[0].Length;
            storage = new T[Rows, Columns];

            foreach (var (row, col) in Cells())
                storage[row, col] = parser(lines[row][col]);
        }

        public IEnumerable<(int row, int col)> FindDifferences(Grid<T> other)
        {
            foreach (var (row, col) in Cells())
                if (!this[row, col].Equals(other[row, col]))
                    yield return (row, col);
        }

        public bool IsValidPosition(int row, int col)
        {
            return row >= 0
                && row < Rows
                && col >= 0
                && col < Columns;
        }

        public Grid<T> Clone()
        {
            var newData = new T[Rows, Columns];
            foreach (var (row, col) in Cells())
                newData[row, col] = storage[row, col];

            return new Grid<T>(newData);
        }

        private Grid(T[,] storage)
        {
            this.storage = storage;
            Rows = storage.GetLength(0);
            Columns = storage.GetLength(1);
        }

        private readonly T[,] storage;
    }

}
