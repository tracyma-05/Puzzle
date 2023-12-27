namespace Puzzle.Core.Figures
{
    /// <summary>
    /// Figure
    /// </summary>
    public partial class FigurePlus : IEquatable<FigurePlus>
    {
        public static FigurePlus[] GetBundles(PuzzleType puzzleType)
        {
            switch (puzzleType)
            {
                case PuzzleType.WithOutWeek:
                    return WithOutWeekBundle;
                case PuzzleType.WithWeek:
                    return WithWeekBundle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(puzzleType));
            }
        }

        /// <summary>
        /// Standard set of puzzle pieces
        /// </summary>
        private static readonly FigurePlus[] WithWeekBundle = {
            new FigurePlus(
                1, 1, 1, 0,
                1, 0, 0, 0,
                1, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 0,
                1, 0, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                0, 1, 1, 0,
                0, 1, 0, 0,
                1, 1, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 0,
                0, 1, 0, 0,
                0, 1, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                0, 0, 1, 1,
                1, 1, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 1,
                1, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 0, 0,
                1, 1, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                0, 1, 1, 0,
                1, 1, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 0, 0, 0,
                1, 0, 0, 0,
                1, 0, 0, 0,
                1, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 0,
                1, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0)
        };

        /// <summary>
        /// Standard set of puzzle pieces
        /// </summary>
        private static readonly FigurePlus[] WithOutWeekBundle = {
            new FigurePlus(
                1, 1, 1, 0,
                1, 0, 0, 0,
                1, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 0,
                1, 0, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                0, 1, 1, 0,
                0, 1, 0, 0,
                1, 1, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 1,
                0, 1, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                0, 0, 1, 1,
                1, 1, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 1,
                1, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 0, 0,
                1, 1, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0),

            new FigurePlus(
                1, 1, 1, 0,
                1, 1, 1, 0,
                0, 0, 0, 0,
                0, 0, 0, 0)
        };
    }
}