using System.Windows.Media;

namespace Puzzle.Models
{
    public class DeskModel
    {
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public Brush Background { get; set; } = Brushes.Transparent;
        public bool Colorful { get; set; }
        public string? Text { get; set; }
    }
}