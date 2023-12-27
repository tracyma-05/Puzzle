using Puzzle.ViewModels;
using System.Windows;

namespace Puzzle
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}