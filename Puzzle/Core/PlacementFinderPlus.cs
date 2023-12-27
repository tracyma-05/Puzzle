using Puzzle.Core.Desks;
using Puzzle.Core.Figures;
using Puzzle.Helpers;
using Puzzle.Models;

namespace Puzzle.Core
{
    public static unsafe class PlacementFinderPlus
    {
        /// <summary>
        /// Finds all suitable placements of pieces on the board (brute force)
        /// </summary>
        public static List<ulong[]> FindAll(DateTime dateTime, PuzzleType puzzleType)
        {
            // create a board with occupied cells
            var markedPoints = DateHelper.GetDateMarkedPoints(dateTime, puzzleType);
            var desk = DeskPlus.CreateDesk(markedPoints, puzzleType);

            var bundles = FigurePlus.GetBundles(puzzleType);
            ulong*[] allFigurePlacements = new ulong*[bundles.Length];
            var allFigurePlacementsCounts = new List<int> { };
            var result = new List<ulong[]>();
            var currentPlacements = new ulong[bundles.Length];
            var index = 0;

            foreach (var item in bundles)
            {
                var figureKindsPlacements = DeskPlus.GetFigureKindsPlacements(desk, item, out var figurePlacementsCount);
#pragma warning disable CA2014
                var figurePlacements = stackalloc ulong[figurePlacementsCount];
#pragma warning restore CA2014
                Copy(figureKindsPlacements, figurePlacements);

                allFigurePlacements[index] = figurePlacements;
                allFigurePlacementsCounts.Add(figurePlacementsCount);
                index++;
            }

            RecursivePlacement(result, desk, allFigurePlacements, allFigurePlacementsCounts.ToArray(), 0, currentPlacements);

            return result;

        }

        #region private

        static void RecursivePlacement(List<ulong[]> result, ulong desk, ulong*[] figurePlacements, int[] figurePlacementsCounts, int depth, ulong[] currentPlacements)
        {
            if (depth == figurePlacements.Length)
            {
                // placed all the figures
                // save a copy of the current placement
                result.Add((ulong[])currentPlacements.Clone());
                MessageHelper.SendNextMessage(MessageType.Status, $"Find one solution, total: {result.Count}");
                return;
            }

            for (var i = 0; i < figurePlacementsCounts[depth]; i++)
            {
                if ((desk & figurePlacements[depth][i]) == 0)
                {
                    desk |= figurePlacements[depth][i];
                    currentPlacements[depth] = figurePlacements[depth][i];

                    RecursivePlacement(result, desk, figurePlacements, figurePlacementsCounts, depth + 1, currentPlacements);

                    // We remove the figure and try further
                    desk &= ~figurePlacements[depth][i];
                    currentPlacements[depth] = 0;
                }
            }
        }

        private static void Copy(List<ulong>[] source, ulong* destination)
        {
            var offset = 0;
            foreach (var figureKindPlacements in source)
            {
                foreach (var placement in figureKindPlacements)
                {
                    destination[offset++] = placement;
                }
            }
        }

        #endregion
    }
}