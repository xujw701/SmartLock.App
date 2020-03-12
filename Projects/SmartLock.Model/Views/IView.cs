using System;
namespace SmartLock.Model.Views
{
    /// <summary>
    /// Describes the base functionality of a view.
    /// </summary>
	public interface IView
    {
        /// <summary>
        /// Shows a busy indicator on the view.
        /// </summary>
		bool IsBusy { get; set; }

        /// <summary>
        /// Raised when the view is closed (going backwards through the stack)
        /// </summary>
        event EventHandler Closed;

        bool DisplayTitle { get; set; }

        string Title { get; set; }

        string Subtitle { get; set; }
    }
}