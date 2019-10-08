using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Controllers;
using PoopSkooter.Services;
using PoopSkooter.Views;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using Serilog;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PoopSkooter {
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication {
        public App() {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU0MDU1QDMxMzcyZTMyMmUzMEJsYlJwMUxzTnlGQ0tocmtCUE1vT2kvT0w3VlFnY1J4d2RNVVlRZksxWmM9");
            InitializeComponent();
        }

        protected override void ConfigureContainer() {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();


            Container.RegisterType<IBackgroundTaskService, BackgroundTaskService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            Container.RegisterType<ITemperatureControllerService, SerialUsbArduinoTemperatureControllerService>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<ITemperatureControllerService, FakeArduinoTemperatureControllerService>(new ContainerControlledLifetimeManager());


            Container.RegisterType<IBeerFactory, BeerFactory>(new ContainerControlledLifetimeManager());

            var temperatureControllerService = Container.Resolve<ITemperatureControllerService>();
            var beerFactory = Container.Resolve<IBeerFactory>();

            Task.Run(() => {
                //Container.Resolve<ITemperatureControllerService>().Run();

                temperatureControllerService.Run();
                //tempertureControllerService.Run();
            });

            //var beerFactory = Container.Resolve<IBeerFactory>();

            Log.Information("Initialization complete.");
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args) {
            await LaunchApplicationAsync(PageTokens.StrikePage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam) {
            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args) {
            await Task.CompletedTask;
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args) {
            base.OnBackgroundActivated(args);
            if (Container == null) {
                // Edge case where the in-process background task's activation trigger is handled when the application is just shut down.
                // Known issue: NullReferenceException in the OnSuspending method for the short application activation to handle the trigger.
                // This will be fixed in the next Prism release, more info see https://github.com/Microsoft/WindowsTemplateStudio/issues/2632
                CreateAndConfigureContainer();
            }

            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args) {
            await base.OnInitializeAsync(args);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) => {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "PoopSkooter.ViewModels.{0}ViewModel, PoopSkooter", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasksAsync();
        }

        protected override IDeviceGestureService OnCreateDeviceGestureService() {
            var service = base.OnCreateDeviceGestureService();
            service.UseTitleBarBackButton = false;
            return service;
        }

        public void SetNavigationFrame(Frame frame) {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame) {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            return shell;
        }
    }
}
