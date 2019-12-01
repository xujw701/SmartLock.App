using System.Threading.Tasks;

namespace SmartLock.Presentation.Core
{
    /// <summary>
    /// Various popup functionalities. 
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Displays a message to the user with a single "OK" button.
        /// </summary>
        /// <param name="title">Title for the message box</param>
        /// <param name="message">Message to display to the user</param>
        void ShowMessage(string title, string message);

        /// <summary>
        /// Awaitable message popup.
        /// </summary>
        /// <param name="title">Title for the message box</param>
        /// <param name="message">Message to display to the user</param>
        Task ShowMessageAsync(string title, string message);

        /// <summary>
        /// Displays an awaitable confirmation dialog.
        /// </summary>
        /// <param name="title">Title for the message box</param>
        /// <param name="message">Message to display to the user</param>
        /// <returns>Whether or not they pressed "OK"</returns>
        Task<bool> ShowOkCancelAsync(string title, string message);

        /// <summary>
        /// Displays an awaitable question to the user with two possible choices.
        /// </summary>
        /// <param name="title">Title for the message box</param>
        /// <param name="message">Message to display to the user</param>
        /// <param name="button1">Choice one</param>
        /// <param name="button2">Choice two</param>
        /// <returns>The selected button</returns>
        Task<MessageBoxButtons> ShowQuestion(string title, string message, string button1, string button2);

        /// <summary>
        /// Displays an awaitable question to the user with two final answers and a neutral choice.
        /// </summary>
        /// <param name="title">Title for the message box</param>
        /// <param name="message">Message to display to the user</param>
        /// <param name="button1">Choice one</param>
        /// <param name="button2">Choice two</param>
        /// <param name="neutralButton">Neutral choice</param>
        /// <returns>The selected button</returns>
        Task<MessageBoxButtons> ShowQuestion(string title, string message, string button1, string button2, string neutralButton);
    }

    /// <summary>
    /// Enum representing the three buttons that can be displayed on a popup.
    /// </summary>
    public enum MessageBoxButtons
    {
        Button1,
        Button2,
        Button
    }
}
