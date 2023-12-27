using System.Windows.Media;

namespace Puzzle.Commons
{
    internal class PuzzleConstants
    {
        internal static Dictionary<string, string> ItemWithOutWeek = new Dictionary<string, string>
        {
            {"0-1","Jan"},
            {"0-2","Feb"},
            {"0-3","Mar"},
            {"0-4","Apr"},
            {"0-5","May"},
            {"0-6","Jun"},
            {"1-1","Jul"},
            {"1-2","Aug"},
            {"1-3","Sep"},
            {"1-4","Oct"},
            {"1-5","Nov"},
            {"1-6","Dec"},

            {"2-1","1"},
            {"2-2","2"},
            {"2-3","3"},
            {"2-4","4"},
            {"2-5","5"},
            {"2-6","6"},
            {"2-7","7"},

            {"3-1","8"},
            {"3-2","9"},
            {"3-3","10"},
            {"3-4","11"},
            {"3-5","12"},
            {"3-6","13"},
            {"3-7","14"},

            {"4-1","15"},
            {"4-2","16"},
            {"4-3","17"},
            {"4-4","18"},
            {"4-5","19"},
            {"4-6","20"},
            {"4-7","21"},

            {"5-1","22"},
            {"5-2","23"},
            {"5-3","24"},
            {"5-4","25"},
            {"5-5","26"},
            {"5-6","27"},
            {"5-7","28"},

            {"6-1","29"},
            {"6-2","30"},
            {"6-3","31"}
        };

        private static Dictionary<string, string> ItemWeekKey = new Dictionary<string, string>
        {
            {"6-4","Sun"},
            {"6-5","Mon"},
            {"6-6","Tue"},
            {"6-7","Wed"},

            {"7-5","Thu"},
            {"7-6","Fri"},
            {"7-7","Sat"},
        };

        internal static Dictionary<string, string> ItemWithWeek = ItemWithOutWeek.Concat(ItemWeekKey).ToDictionary(k => k.Key, v => v.Value);

        internal static Dictionary<int, Brush> ColorMapping = new Dictionary<int, Brush>
        {
            { 0, Brushes.Red },
            { 1, Brushes.Orange },
            { 2, Brushes.Yellow },
            { 3, Brushes.Green },
            { 4, Brushes.Blue },
            { 5, Brushes.Brown },
            { 6, Brushes.Purple },
            { 7, Brushes.Pink },
            { 8, Brushes.Orchid },
            { 9, Brushes.Cyan },
        };
    }
}