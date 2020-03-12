using System;
using System.Threading.Tasks;
using Android.App;
using SmartLock.Presentation.Droid.Views.ViewBases;
using SmartLock.Model.Services;

namespace SmartLock.Presentation.Droid.Platform
{
    public class MessageBoxService : IMessageBoxService
    {
        public void ShowMessage(string title, string message)
        {
            var currentActivity = ViewBase.CurrentActivity;
            if (currentActivity == null) return;
            
            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetPositiveButton("OK", (sender, args) => { ((Dialog)sender).Dismiss(); });
            builder.Show();
        }

        public Task ShowMessageAsync(string title, string message)
        {
            var tsc = new TaskCompletionSource<bool>();

            var currentActivity = ViewBase.CurrentActivity;
            if (currentActivity == null) return tsc.Task;

            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetPositiveButton("OK", (sender, args) => { ((Dialog)sender).Dismiss(); tsc.TrySetResult(false); });
            builder.Show();

            return tsc.Task;
        }

        public Task<bool> ShowOkCancelAsync(string title, string message)
        {
            var tsc = new TaskCompletionSource<bool>();

            var currentActivity = ViewBase.CurrentActivity;

            if (currentActivity == null)
            {
                tsc.TrySetResult(false);
                return tsc.Task;
            }

            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetPositiveButton("OK", (sender, args) => { ((Dialog)sender).Dismiss(); tsc.TrySetResult(true); });
            builder.SetNegativeButton("Cancel", (sender, args) => { ((Dialog)sender).Cancel(); tsc.TrySetResult(false); });
            builder.Show();

            return tsc.Task; // equivalent to OK
        }

        public async Task<MessageBoxButtons> ShowQuestion(string title, string message, string button1, string button2)
        {
            var tsc = new TaskCompletionSource<MessageBoxButtons>();

            var currentActivity = ViewBase.CurrentActivity;
            if (currentActivity == null)
            {
                tsc.TrySetResult(MessageBoxButtons.Button2);
                return await tsc.Task;
            }

            var builder = new AlertDialog.Builder(currentActivity);

            if (string.IsNullOrEmpty(message))
            {
                builder.SetMessage(title);
            }
            else
            {
                builder.SetMessage(message);
                builder.SetTitle(title);
            }

            builder.SetNegativeButton(button1.ToLower(), (sender, args) => { ((Dialog)sender).Cancel(); tsc.TrySetResult(MessageBoxButtons.Button1); });
            builder.SetPositiveButton(button2.ToLower(), (sender, args) => { ((Dialog)sender).Dismiss(); tsc.TrySetResult(MessageBoxButtons.Button2); });
            builder.SetCancelable(false);

            var dialog = builder.Show();
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
            
            return await tsc.Task;
        }

        public Task<MessageBoxButtons> ShowQuestion(string title, string message, string button1, string button2, string neutralButton)
        {
            throw new NotImplementedException();
        }
    }
}