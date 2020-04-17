using System.Linq;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.Views;

namespace SmartLock.Presentation.Core.ViewControllers
{
    public class EnvironmentController : ViewController<IEnvironmentView>
    {
        private readonly IEnvironmentManager _environmentManager;

        public EnvironmentController(IViewService viewService, IEnvironmentManager environmentManager) : base(viewService)
        {
            _environmentManager = environmentManager;
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.BackClick += () => Pop();
            View.EnvironemntChanged += (env) =>
            {
                _environmentManager.SelectedEnvironment = _environmentManager.Environments.FirstOrDefault(e => e.Name.Equals(env));
            };

            View.Show(_environmentManager.Environments.Select(env => env.Name).ToList(), _environmentManager.SelectedEnvironment.Name);
        }
    }
}
