using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Puzzle.Models;
using System.Windows;

namespace Puzzle.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient, IRecipient<PuzzleMessage>
    {
        public MainWindowViewModel()
        {
            WeakReferenceMessenger.Default.Register(this);
        }

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _status;

        public void Receive(PuzzleMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.Status:
                    Status = message.Content.ToString()!;
                    break;
                case MessageType.IsBusy:
                    IsBusy = bool.Parse(message.Content.ToString()!);
                    break;
                case MessageType.Info:
                    MessageBox.Show(message.Content.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}