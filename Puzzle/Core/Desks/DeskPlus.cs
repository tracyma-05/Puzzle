using Puzzle.Core.Figures;
using Puzzle.Models;
using System.Text;

namespace Puzzle.Core.Desks
{
    /// <summary>
    /// Board on which the pieces are placed
    /// </summary>
    public static class DeskPlus
    {
        public const int Size = 8;

        /// <summary>
        /// 00011111
        /// 00000001
        /// 00000001
        /// 00000001
        /// 00000001
        /// 00000001
        /// 10000001
        /// 10000001
        /// </summary>
        private static ulong SeedWithWeek = 2234067993975947649L;

        /// <summary>
        /// 11111111
        /// 11110001
        /// 00000001
        /// 00000001
        /// 00000001
        /// 00000001
        /// 10000001
        /// 10000001
        /// </summary>
        private static ulong SeedWithOutWeek = 18442523052882362753L;

        public static ulong CreateDesk(IEnumerable<DeskPoint> markedPoints, PuzzleType puzzle)
        {
            var result = puzzle == PuzzleType.WithWeek ? SeedWithWeek : SeedWithOutWeek;
            if (!markedPoints.Any())
            {
                return result;
            }

            ulong raw = 1;
            foreach (var item in markedPoints)
            {
                result = result | raw << item.Row * Size + item.Column + Size - 7;
            }

            return result;
        }

        public static List<ulong>[] GetFigureKindsPlacements(ulong desk, FigurePlus figure, out int allPlacementsCount)
        {
            // we get different shapes
            var figureKinds = figure.GetKinds();

            // for each variety we obtain its possible positions on the board
            var figureKindsPlacements = new List<ulong>[figureKinds.Count];
            allPlacementsCount = 0;
            for (var i = 0; i < figureKinds.Count; i++)
            {
                var placements = GetFigurePlacements(desk, figureKinds[i]);
                allPlacementsCount += placements.Count;
                figureKindsPlacements[i] = placements;
            }

            return figureKindsPlacements;
        }

        /// <summary>
        /// Returns theoretical options for placing a piece on the board
        /// </summary>
        private static List<ulong> GetFigurePlacements(ulong desk, FigurePlus figure)
        {
            // we generate all possible placements of the figure on the board, taking into account the dimensions
            var rows = Size + 1 - figure.Height;
            var columns = Size + 1 - figure.Width;

            var placements = new List<ulong>(rows * columns);
            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                {
                    var figureMask = figure.GetMask(row, column);
                    if ((desk & figureMask) == 0) // a piece in this position can be placed on the board
                        placements.Add(figureMask);
                }

            return placements;
        }


        public static bool CanPlaceFigure(ulong desk, FigurePlus figure, int row, int column)
        {
            // check that space for the figure is free
            // if there is an intersection of bits, i.e. some board cells are already occupied
            return (desk & figure.GetMask(row, column)) == 0;
        }

        public static ulong PlaceFigure(ulong desk, FigurePlus figure, int row, int column)
        {
            return desk | figure.GetMask(row, column);
        }


        public static string ToString(ulong[] placements)
        {
            var board = new char[Size * Size];
            ulong raw = 1;
            for (var i = 0; i < board.Length; i++)
            {
                board[i] = ' ';
            }

            // draw each piece on the board with the letters A, B, C, ...

            for (var i = 0; i < placements.Length; i++)
            {
                var figureTemplate = (char)('A' + i);
                var placementBitmask = placements[i];

                for (var j = 0; j < Size * Size; j++)
                {
                    // if the i-th bit is in the mask, draw a figure template in the corresponding cell
                    var mask = raw << j;
                    if ((placementBitmask & mask) == mask)
                        board[j] = figureTemplate;
                }
            }

            // draw the boundaries of shapes using Unicode characters: ─ │ ┌ ┐ └ ┘ ├ ┤ ┬ ┴ ┼

            var builder = new StringBuilder();
            for (var i = 0; i <= Size; i++)
            {
                for (var j = 0; j <= Size; j++)
                    builder.Append(GetCornerChar(board, i, j));
                builder.AppendLine();
            }
            return builder.ToString();
        }

        /// <summary>
        /// Draws the upper left corner of the specified cell with Unicode characters: ─ │ ┌ ┐ └ ┘ ├ ┤ ┬ ┴ ┼
        /// </summary>
        private static string GetCornerChar(char[] board, int row, int column)
        {
            // draw the upper left corner for the specified cell (the cell may extend beyond the right and bottom borders)
            const char absent = ' '; // missing cell

            var topLeft = row == 0 || column == 0 ? absent : board[(row - 1) * Size + column - 1];
            var topRight = row == 0 || column == Size ? absent : board[(row - 1) * Size + column];
            var bottomLeft = row == Size || column == 0 ? absent : board[row * Size + column - 1];
            var bottomRight = row == Size || column == Size ? absent : board[row * Size + column];

            if (topLeft == topRight)
            {
                if (bottomLeft == bottomRight)
                {
                    if (topLeft == bottomLeft)
                        return "  ";
                    else
                        return "──";
                }
                else // bottomLeft != bottomRight
                {
                    if (topLeft == bottomLeft)
                        return "┌─";
                    else if (topRight == bottomRight)
                        return "┐ ";
                    else
                        return "┬─";
                }
            }
            else // topLeft != topRight
            {
                if (bottomLeft == bottomRight)
                {
                    if (topLeft == bottomLeft)
                        return "└─";
                    else if (topRight == bottomRight)
                        return "┘ ";
                    else
                        return "┴─";
                }
                else // bottomLeft != bottomRight
                {
                    if (topLeft == bottomLeft)
                    {
                        if (topRight == bottomRight)
                            return "│ ";
                        else
                            return "├─";
                    }
                    else
                    {
                        if (topRight == bottomRight)
                            return "┤ ";
                        else
                            return "┼─";
                    }
                }
            }
        }
    }
}