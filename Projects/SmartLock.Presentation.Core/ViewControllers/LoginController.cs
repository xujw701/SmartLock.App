using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;
using System.Threading.Tasks;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class LoginController : ViewController<ILoginView>
    {
        public LoginController(IViewService viewService) : base(viewService)
        {
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.LoginClicked += () => Push<MainController>();
        }

        //private async Task Login()
        //{
        //    await Task.Delay(300);

        //    Push<MainController>();
        //}
    }
}
