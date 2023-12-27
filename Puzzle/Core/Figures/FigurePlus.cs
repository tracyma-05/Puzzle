using Puzzle.Core.Desks;
using System.Text;

namespace Puzzle.Core.Figures
{
    public partial class FigurePlus : IEquatable<FigurePlus>
    {
        private const int Size = 4;

        public readonly byte[] _rows; // 4 lines, each line is a bitmask of 4 cells

        public readonly int Width;
        public readonly int Height;

        public FigurePlus(params byte[] points)
        {
            if (points.Length != Size * Size)
                throw new ArgumentException("Must be " + Size * Size + " Points", nameof(points));
            _rows = new byte[Size];

            for (var i = 0; i < Size; i++)
                _rows[i] = (byte)(points[i * Size] | (points[i * Size + 1] << 1) | (points[i * Size + 2] << 2) | (points[i * Size + 3] << 3));

            Width = 1;
            Height = 1;

            // first row and first column must be non-empty
            if (IsRowEmpty(0))
                throw new ArgumentException("The first line is empty");
            if (IsColumnEmpty(0))
                throw new ArgumentException("The first column is empty");

            while (Width < Size && !IsColumnEmpty(Width))
                Width++;

            while (Height < Size && !IsRowEmpty(Height))
                Height++;
        }

        public bool IsMarked(int rowIndex, int columnIndex)
        {
            var mask = (byte)(1 << columnIndex);
            return (_rows[rowIndex] & mask) == mask;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var rowsExists = false;

            for (var i = 0; i < Size; i++)
            {
                var isEmpty = IsRowEmpty(i);
                if (isEmpty)
                    continue;

                if (rowsExists)
                    builder.AppendLine();

                for (var j = 0; j < Size; j++)
                {
                    var ch = IsMarked(i, j) ? 'X' : ' ';
                    builder.Append(ch);
                }

                // remove trailing spaces
                var lastIndex = builder.Length - 1;
                while (builder[lastIndex] == ' ')
                    builder.Remove(lastIndex--, 1);

                rowsExists = true;
            }

            return builder.ToString();
        }

        public ulong GetMask(int row, int column)
        {
            // calculate the bitmask of a piece located on the board
            ulong result = 0;
            var offset = row * DeskPlus.Size + column; // TODO Size=8
            for (var i = 0; i < Height; i++, offset += DeskPlus.Size)
                result |= (ulong)_rows[i] << offset;
            return result;
        }

        public bool Equals(FigurePlus other)
        {
            for (var i = 0; i < _rows.Length; i++)
                if (_rows[i] != other._rows[i])
                    return false;
            return true;
        }

        public override int GetHashCode()
        {
            // fold as a bitmask
            int result = _rows[0];
            for (var i = 1; i < _rows.Length; i++)
            {
                result <<= Size;
                result |= _rows[i];
            }

            return result;
        }

        /// <summary>
        /// Returns an inverted figure
        /// </summary>
        public FigurePlus GetTurned()
        {
            // looking for the rightmost non-empty column
            var lastColumnIndex = Width - 1;

            var points = new byte[Size * Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j <= lastColumnIndex; j++)
                {
                    var from = j;
                    var to = lastColumnIndex - from;
                    if (IsMarked(i, from))
                        points[i * Size + to] = 1;
                }
            }

            return new FigurePlus(points);
        }

        /// <summary>
        /// Returns a shape rotated 90 degrees clockwise
        /// </summary>
        public FigurePlus GetRotated90()
        {
            var points = new byte[Size * Size];
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    if (IsMarked(i, j))
                    {
                        var newI = j;
                        var newJ = Size - 1 - i;
                        points[newI * Size + newJ] = 1;
                    }
                }
            }

            // remove empty columns on the left if they appear
            var columnStartIndex = 0;
            while (IsColumnEmpty(points, columnStartIndex))
                columnStartIndex++;
            if (columnStartIndex > 0)
            {
                for (var i = 0; i < Size; i++)
                    for (var j = 0; j < Size; j++)
                    {
                        if (j + columnStartIndex < Size)
                            points[i * Size + j] = points[i * Size + j + columnStartIndex];
                        else
                            points[i * Size + j] = 0;
                    }
            }

            return new FigurePlus(points);
        }

        /// <summary>
        /// Returns its unique varieties (turns, flip to the other side)
        /// </summary>
        public List<FigurePlus> GetKinds()
        {
            // The shape can be rotated clockwise (4 times),
            // and also flip to the other side.
            // Thus, a figure can have a maximum of 8 varieties
            var kinds = new List<FigurePlus>();
            kinds.Add(this);

            var rotated90 = GetRotated90();
            if (!kinds.Contains(rotated90))
                kinds.Add(rotated90);

            var rotated180 = rotated90.GetRotated90();
            if (!kinds.Contains(rotated180))
                kinds.Add(rotated180);

            var rotated270 = rotated180.GetRotated90();
            if (!kinds.Contains(rotated270))
                kinds.Add(rotated270);

            var turned = GetTurned();
            if (kinds.Contains(turned))
            {
                // if the inverted figure is already in the variations, then all the options for its rotations have already been added
                return kinds;
            }

            kinds.Add(turned);

            var turnedAndRotated90 = turned.GetRotated90();
            if (!kinds.Contains(turnedAndRotated90))
                kinds.Add(turnedAndRotated90);

            var turnedAndRotated180 = turnedAndRotated90.GetRotated90();
            if (!kinds.Contains(turnedAndRotated180))
                kinds.Add(turnedAndRotated180);

            var turnedAndRotated270 = turnedAndRotated180.GetRotated90();
            if (!kinds.Contains(turnedAndRotated270))
                kinds.Add(turnedAndRotated270);

            return kinds;
        }

        public override bool Equals(object obj)
        {
            return obj is FigurePlus other && Equals(other);
        }

        private bool IsRowEmpty(int rowIndex)
        {
            for (var j = 0; j < Size; j++)
            {
                if (IsMarked(rowIndex, j))
                    return false;
            }
            return true;
        }

        private bool IsColumnEmpty(int columnIndex)
        {
            for (var i = 0; i < Size; i++)
            {
                if (IsMarked(i, columnIndex))
                    return false;
            }
            return true;
        }

        private static bool IsColumnEmpty(byte[] points, int columnIndex)
        {
            for (var i = 0; i < Size; i++)
            {
                if (points[i * Size + columnIndex] == 1)
                    return false;
            }
            return true;
        }
    }
}