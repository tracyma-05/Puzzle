namespace Puzzle.Models
{
    public class PuzzleMessage
    {
        public MessageType MessageType { get; set; }
        public object Content { get; set; } = null!;
    }
}