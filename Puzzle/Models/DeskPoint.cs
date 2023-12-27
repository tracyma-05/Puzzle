namespace Puzzle.Models
{
    public class DeskPoint
    {
        public DeskPoint(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}