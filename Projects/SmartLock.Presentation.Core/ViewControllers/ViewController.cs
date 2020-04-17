using System;
using System.Net;
using System.Threading.Tasks;
using SmartLock.Infrastructure;
using SmartLock.Model.Server;
using SmartLock.Model.Services;
using SmartLock.Model.Views;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public abstract class ViewController
    {
        public abstract void SetView(object obj);
        public abstract void NotifyViewWillShow(IView view);
        public abstract void Pop();
    }

    public abstract class ViewController<T> : ViewController where T : class, IView
    {
        private T _view;

        protected ViewController(IViewService viewService)
        {
            ViewService = viewService;
        }

        protected T View => _view;

        protected IViewService ViewService { get; }

        protected virtual bool AutomaticallySetsHeaderColor => true;

        protected void Push<TViewController>(Action<TViewController> viewControllerCreated = null, bool animated = true) where TViewController : class
        {
            ViewService.Push(viewControllerCreated, animated);
        }

        public override void Pop()
        {
            ViewService.Pop();
        }

        protected void PopToRoot()
        {
            ViewService.PopToRoot();
        }

        public override void SetView(object view)
        {
            _view = (T)view;
            _view.Closed += (s, e) => NotifyViewWillUnload();

            OnViewLoaded();
        }

        public override void NotifyViewWillShow(IView view)
        {
            OnViewWillShow();
        }

        protected virtual void OnViewLoaded()
        {

        }

        protected virtual void OnViewWillShow()
        {

        }

        private void NotifyViewWillUnload()
        {
            OnViewClosed();
        }

        protected virtual void OnViewClosed()
        {

        }

        protected bool ViewIsBusy
        {
            get => View.IsBusy;
            set
            {
                View.IsBusy = value;
                OnViewIsBusyChanged();
            }
        }

        protected virtual void OnViewIsBusyChanged()
        {

        }

        /// <summary>
        /// Performs an action safely, will handle exceptions and display error message.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="setIsBusy">Will set IsBusy on the view.</param>
        protected void DoSafe(Action action, bool setIsBusy = true, Action<Exception> failedAction = null, Action successAcction = null)
        {
            Exception exception = null;

            try
            {
                if (setIsBusy)
                {
                    ViewIsBusy = true;
                }

                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (setIsBusy && ViewIsBusy)
                {
                    ViewIsBusy = false;
                }
            }

            if (exception != null)
            {
                ShowError(exception);
                failedAction?.Invoke(exception);
            }
            else
            {
                successAcction?.Invoke();
            }
        }

        /// <summary>
        /// Performs an action safely, will handle exceptions and display error message.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="setIsBusy">Will set IsBusy on the view.</param>
        /// <param name="failedAction">Action to call if an exception occurs on action. Can be null.</param>
        /// <param name="successAcction">Action to call if no exception occurs on action. Can be null.</param>
        protected async void DoSafeAsync(Func<Task> action, bool setIsBusy = true, Action<Exception> failedAction = null, Action successAcction = null)
        {
            Exception exception = null;

            try
            {
                if (setIsBusy)
                {
                    ViewIsBusy = true;
                }

                // await Task.Factory.StartNew(action);
                await action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (setIsBusy && ViewIsBusy)
                {
                    ViewIsBusy = false;
                }
            }

            if (exception != null)
            {
                await ShowErrorAsync(exception);
                failedAction?.Invoke(exception);
            }
            else
            {
                successAcction?.Invoke();
            }
        }

        protected virtual void ShowError(Exception exception)
        {
            IoC.Resolve<IMessageBoxService>().ShowMessageAsync("Error", exception.Message);
        }

        protected virtual async Task ShowErrorAsync(Exception exception)
        {
            var messageBoxService = IoC.Resolve<IMessageBoxService>();

            if (exception is WebServiceClientException webServiceClientException)
            {
                if (webServiceClientException.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await messageBoxService.ShowMessageAsync("Tips", "Your credentials could not be authenticated. Please log in again.");
                    await IoC.Resolve<IUserService>().LogOut();
                    ViewService.PopToRoot();
                }
                else
                {
                    await messageBoxService.ShowMessageAsync("Error", exception.Message);
                }
            }
            else if (exception is TaskCanceledException)
            {
                // Do nothing
            }
            else
            {
                //await messageBoxService.ShowMessageAsync("Error", exception.Message);
            }
        }
    }
}
