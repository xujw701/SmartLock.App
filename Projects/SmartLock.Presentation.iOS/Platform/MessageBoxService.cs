using System.Threading.Tasks;
using SmartLock.Model.Services;
using UIKit;

namespace SmartLock.Presentation.iOS.Platform
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessage(string title, string message)
        {
            var alert = new UIAlertView();
            alert.Title = title;
            alert.Message = message;
            alert.AddButton("OK");
            alert.Show();
        }

        public Task ShowMessageAsync(string title, string message)
        {
            var alert = new UIAlertView();
            alert.Title = title;
            alert.Message = message;
            alert.AddButton("OK");

            var tsc = new TaskCompletionSource<bool>();
            alert.Clicked += (sender, buttonArgs) => {
                if (buttonArgs.ButtonIndex == 0)
                    tsc.TrySetResult(true);
            };

            alert.Show();
            return tsc.Task;
        }

        public Task<bool> ShowOkCancelAsync(string title, string message)
        {
            var alert = new UIAlertView();
            alert.Title = title;
            alert.Message = message;
            alert.AddButton("OK");
            alert.AddButton("Cancel");
            alert.Show();

            var tsc = new TaskCompletionSource<bool>();
            alert.Clicked += (sender, buttonArgs) => {
                tsc.TrySetResult(buttonArgs.ButtonIndex == 0);
            };

            return tsc.Task;
        }

        public void Test()
        {

        }

        public Task<MessageBoxButtons> ShowQuestion(string title, string message, string button1, string button2)
        {
            var alert = new UIAlertView();
            alert.Title = title;
            alert.Message = message;
            alert.AddButton(button1);
            alert.AddButton(button2);
            alert.Show();

            var tsc = new TaskCompletionSource<MessageBoxButtons>();
            alert.Clicked += (sender, buttonArgs) => {
                switch (buttonArgs.ButtonIndex)
                {
                    case 0:
                        tsc.TrySetResult(MessageBoxButtons.Button1);
                        break;
                    case 1:
                        tsc.TrySetResult(MessageBoxButtons.Button2);
                        break;
                }
            };

            return tsc.Task;
        }

        public Task<MessageBoxButtons> ShowQuestion(string title, string message, string button1, string button2, string neutralButton)
        {
            var alert = new UIAlertView();
            alert.Title = title;
            alert.Message = message;
            alert.AddButton(button1);
            alert.AddButton(button2);
            alert.Show();

            var tsc = new TaskCompletionSource<MessageBoxButtons>();
            alert.Clicked += (sender, buttonArgs) => {
                switch (buttonArgs.ButtonIndex)
                {
                    case 0:
                        tsc.TrySetResult(MessageBoxButtons.Button1);
                        break;
                    case 1:
                        tsc.TrySetResult(MessageBoxButtons.Button2);
                        break;
                    case 2:
                        tsc.TrySetResult(MessageBoxButtons.Button2);
                        break;
                }
            };

            return tsc.Task;
        }
    }
}
