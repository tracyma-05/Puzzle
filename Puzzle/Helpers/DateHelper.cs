using Puzzle.Core;
using Puzzle.Models;

namespace Puzzle.Helpers
{
    public static class DateHelper
    {
        public static IEnumerable<DeskPoint> GetDateMarkedPoints(this DateTime dt, PuzzleType puzzleType = PuzzleType.WithWeek)
        {

            TryGetDayCell(dt.Day, out var rowDay, out var colDay);
            TryGetMonthCell(dt.Month, out var rowMonth, out var colMonth);
            TryGetWeekCell(dt.DayOfWeek, out var rowWeek, out var colWeek);
            var result = new List<DeskPoint>
            {
                new DeskPoint(rowDay, colDay),
                new DeskPoint(rowMonth, colMonth)
            };

            switch (puzzleType)
            {
                case PuzzleType.WithOutWeek:
                    
                    break;
                case PuzzleType.WithWeek:
                    result.Add(new DeskPoint(rowWeek, colWeek));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(puzzleType));
            }

            return result;
        }

        private static bool TryGetMonthCell(int month, out int rowIndex, out int columnIndex)
        {
            if (month >= 1 && month <= 12)
            {
                rowIndex = Math.DivRem(month - 1, 6, out columnIndex);
                return true;
            }

            rowIndex = columnIndex = 0;
            return false;
        }

        private static bool TryGetDayCell(int day, out int rowIndex, out int columnIndex)
        {
            if (day >= 1 && day <= 31)
            {
                rowIndex = Math.DivRem(day - 1, 7, out columnIndex) + 2;
                return true;
            }

            rowIndex = columnIndex = 0;
            return false;
        }

        private static bool TryGetWeekCell(DayOfWeek week, out int rowWeekIndex, out int columnWeekIndex)
        {
            switch (week)
            {
                case DayOfWeek.Sunday:
                    rowWeekIndex = 6;
                    columnWeekIndex = 3;
                    break;
                case DayOfWeek.Monday:
                    rowWeekIndex = 6;
                    columnWeekIndex = 4;
                    break;
                case DayOfWeek.Tuesday:
                    rowWeekIndex = 6;
                    columnWeekIndex = 5;
                    break;
                case DayOfWeek.Wednesday:
                    rowWeekIndex = 6;
                    columnWeekIndex = 6;
                    break;
                case DayOfWeek.Thursday:
                    rowWeekIndex = 7;
                    columnWeekIndex = 4;
                    break;
                case DayOfWeek.Friday:
                    rowWeekIndex = 7;
                    columnWeekIndex = 5;
                    break;
                case DayOfWeek.Saturday:
                    rowWeekIndex = 7;
                    columnWeekIndex = 6;
                    break;
                default:
                    rowWeekIndex = 0;
                    columnWeekIndex = 0;
                    return false;
            }

            return true;
        }
    }
}