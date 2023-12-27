using Puzzle.Commons;
using Puzzle.Core;
using Puzzle.Core.Desks;
using Puzzle.Helpers;
using Puzzle.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Puzzle.UserControls
{
    public partial class PuzzleDesk : UserControl
    {
        #region private

        /// <summary>
        /// is generate label
        /// </summary>
        private bool _isInitLabel;

        /// <summary>
        /// result index
        /// </summary>
        private int _index = 0;

        /// <summary>
        /// store the results
        /// </summary>
        private List<ulong[]> _results;

        #endregion

        public PuzzleDesk()
        {
            InitializeComponent();
            CustomInit();
        }

        #region event handler

        /// <summary>
        /// puzzle type changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void puzzleTypeCbb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentDay = (DateTime)datePicker.SelectedDate!;
            var labelModels = GetDeskModels(currentDay);
            UpdateDeskInfo(labelModels);
        }

        /// <summary>
        /// date select changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // date actual changed
            var currentDay = (DateTime)e.AddedItems[0]!;
            var oldItems = e.RemovedItems;
            if (oldItems != null && oldItems.Count > 0)
            {
                var old = (DateTime)oldItems[0]!;
                if (currentDay.Date == old.Date && _isInitLabel)
                {
                    // if no actual changed, then do nothing.
                    return;
                }
            }

            var labelModels = GetDeskModels(currentDay);
            // generate label
            if (!_isInitLabel)
            {
                foreach (var model in labelModels)
                {
                    var label = new Label
                    {
                        Name = $"desk_{model.RowNumber}_{model.ColumnNumber}",
                        Background = model.Background,
                        Content = model.Text,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };

                    Grid.SetRow(label, model.RowNumber);
                    Grid.SetColumn(label, model.ColumnNumber);

                    containGrid.Children.Add(label);

                }

                _isInitLabel = true;
            }
            else
            {
                UpdateDeskInfo(labelModels);
            }
        }

        /// <summary>
        /// to find the solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageHelper.SendNextMessage(MessageType.IsBusy, true);
            var puzzleType = GetCurrentPuzzleType();
            var day = (DateTime)datePicker.SelectedDate!;
            var models = await GetFigureModelAsync(day, puzzleType);
            if (models == null || !models.Any())
            {
                return;
            }

            if (models.Count > 2)
            {
                preBtn.IsEnabled = false;
                nextBtn.IsEnabled = true;
            }

            UpdateDeskBackground(models);
            MessageHelper.SendNextMessage(MessageType.IsBusy, false);
        }


        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_index == 0)
            {
                preBtn.IsEnabled = false;
                MessageBox.Show("That the first one.");
                return;
            }

            _index--;
            var models = GetDeskModels(_index);
            UpdateDeskBackground(models);
            if (_index <= 0)
            {
                _index = 0;
            }

            if (_index <= _results.Count)
            {
                nextBtn.IsEnabled = true;
            }

            MessageHelper.SendNextMessage(MessageType.Status, $"{datePicker.SelectedDate?.ToString("yyyy-MM-dd")} total solutions is: {_results.Count}, current: {_index + 1}");
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_index == _results.Count - 1)
            {
                nextBtn.IsEnabled = false;
                MessageBox.Show("That the last one.");
                return;
            }

            _index++;
            var models = GetDeskModels(_index);
            UpdateDeskBackground(models);
            if (_index >= _results.Count)
            {
                _index = _results.Count;
            }

            if (_index >= 1)
            {
                preBtn.IsEnabled = true;
            }

            MessageHelper.SendNextMessage(MessageType.Status, $"{datePicker.SelectedDate?.ToString("yyyy-MM-dd")} total solutions is: {_results.Count}, current: {_index + 1}");
        }

        #endregion

        #region private

        /// <summary>
        /// init custom control status
        /// </summary>
        private void CustomInit()
        {
            // init date
            datePicker.SelectedDate = DateTime.Now;

            // init puzzle type
            puzzleTypeCbb.Items.Add(PuzzleType.WithWeek.ToString());
            puzzleTypeCbb.Items.Add(PuzzleType.WithOutWeek.ToString());

            puzzleTypeCbb.SelectedItem = PuzzleType.WithWeek.ToString();

            // init button status
            preBtn.IsEnabled = false;
            nextBtn.IsEnabled = false;
        }

        private Brush InitDeskBackground(string val, DeskPoint currentPoint, IEnumerable<DeskPoint> markedPoints)
        {
            if (val == "0")
            {
                return Brushes.Transparent;
            }

            foreach (var item in markedPoints)
            {
                if (currentPoint.Row == item.Row && currentPoint.Column == item.Column + 1)
                {
                    return Brushes.LightGray;
                }
            }

            return Brushes.Gray;
        }

        private PuzzleType GetCurrentPuzzleType()
        {
            var isSuccess = Enum.TryParse(typeof(PuzzleType), puzzleTypeCbb.SelectedItem?.ToString(), true, out var puzzleType);
            if (isSuccess)
            {
                return (PuzzleType)puzzleType!;
            }

            return PuzzleType.WithWeek;
        }

        private async Task<List<DeskModel>?> GetFigureModelAsync(DateTime day, PuzzleType puzzleType, int resultIndex = 0)
        {
            return await Task.Run(() =>
            {
                var resultPlus = PlacementFinderPlus.FindAll(day, puzzleType);
                if (resultPlus.Any())
                {
                    _results = resultPlus;
                    var solution1 = _results[resultIndex];
                    var models = new List<DeskModel>();
                    var index = 0;
                    foreach (var solution in solution1)
                    {
                        var bin = solution.ToString("b").PadLeft(64, '0');
                        for (int i = 0; i < bin.Count(); i++)
                        {
                            var item = bin[i].ToString();
                            if (item == "1")
                            {
                                var row = 7 - i / 8;
                                var column = 7 - i % 8;

                                PuzzleConstants.ColorMapping.TryGetValue(index, out var color);
                                models.Add(new DeskModel
                                {
                                    RowNumber = row,
                                    ColumnNumber = column,
                                    Background = color!,
                                    Colorful = true
                                });
                            }
                        }

                        index++;
                    }

                    return models;
                }

                return null;
            });
        }

        private void UpdateDeskBackground(IEnumerable<DeskModel> models)
        {
            foreach (var item in models)
            {
                for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(containGrid) - 1; i++)
                {
                    var child = VisualTreeHelper.GetChild(containGrid, i);
                    var label = child as Label;
                    if (label != null && label.Name == $"desk_{item.RowNumber}_{item.ColumnNumber}")
                    {
                        label.Background = item.Background;
                        label.Content = item.Text;
                    }
                }
            }
        }

        private IEnumerable<DeskModel> GetDeskModels(int currentIndex)
        {
            var current = _results[currentIndex];
            var models = new List<DeskModel>();
            var index = 0;
            foreach (var solution in current)
            {
                var bin = solution.ToString("b").PadLeft(64, '0');
                for (int i = 0; i < bin.Count(); i++)
                {
                    var item = bin[i].ToString();
                    if (item == "1")
                    {
                        var row = 7 - i / 8;
                        var column = 7 - i % 8;

                        PuzzleConstants.ColorMapping.TryGetValue(index, out var color);
                        models.Add(new DeskModel
                        {
                            RowNumber = row,
                            ColumnNumber = column,
                            Background = color!,
                            Colorful = true
                        });
                    }
                }

                index++;
            }

            return models;
        }

        private IEnumerable<DeskModel> GetDeskModels(DateTime currentDay)
        {
            // date changed, then init controls
            preBtn.IsEnabled = false;
            nextBtn.IsEnabled = false;
            _index = 0;
            _results = new List<ulong[]>();

            var puzzleType = GetCurrentPuzzleType();
            var markedPoints = DateHelper.GetDateMarkedPoints(currentDay, puzzleType);
            var desk = DeskPlus.CreateDesk(markedPoints, puzzleType);

            // long to binary
            var deskBinary = desk.ToString("b").PadLeft(64, '0');
            var labelModels = new List<DeskModel>();
            for (int i = 0; i < deskBinary.Count(); i++)
            {
                var item = deskBinary[i];
                var row = 7 - i / 8;
                var column = 7 - i % 8;

                var currentPoint = new DeskPoint(row, column);
                var keyDic = puzzleType == PuzzleType.WithWeek ? PuzzleConstants.ItemWithWeek : PuzzleConstants.ItemWithOutWeek;
                labelModels.Add(new DeskModel
                {
                    RowNumber = row,
                    ColumnNumber = column,
                    Background = InitDeskBackground(item.ToString(), currentPoint, markedPoints),
                    Colorful = item.ToString() == "1",
                    Text = keyDic.GetValueOrDefault($"{row}-{column}")
                });
            }

            return labelModels;
        }

        private void UpdateDeskInfo(IEnumerable<DeskModel> models)
        {
            // update label color
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(containGrid) - 1; i++)
            {
                var child = VisualTreeHelper.GetChild(containGrid, i);
                var label = child as Label;
                if (label != null)
                {
                    label.Background = Brushes.Transparent;
                }
            }

            UpdateDeskBackground(models);
        }

        #endregion
    }
}