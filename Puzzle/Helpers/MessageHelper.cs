using CommunityToolkit.Mvvm.Messaging;
using Puzzle.Models;

namespace Puzzle.Helpers
{
    public class MessageHelper
    {
        public static void SendNextMessage(MessageType messageType, object context)
        {
            WeakReferenceMessenger.Default.Send(new PuzzleMessage
            {
                MessageType = messageType,
                Content = context
            });
        }
    }
}