using System;
using System.Globalization;
using System.Threading.Tasks;

using Humpty.Services;
using Humpty.Views;

using Microsoft.Practices.Unity;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Controllers;
using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using Serilog;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Humpty {
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
            await LaunchApplicationAsync(PageTokens.StrikeWaterHeatPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam) {
            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args) {
            await Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args) {
            await base.OnInitializeAsync(args);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) => {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Humpty.ViewModels.{0}ViewModel, Humpty", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
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
