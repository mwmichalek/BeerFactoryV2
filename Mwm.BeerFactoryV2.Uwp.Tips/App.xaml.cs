﻿using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Controllers;
using Mwm.BeerFactoryV2.Service.Phases;
using Mwm.BeerFactoryV2.Uwp.Tips.BackgroundTasks;
using Mwm.BeerFactoryV2.Uwp.Tips.Services;
using Mwm.BeerFactoryV2.Uwp.Tips.ViewModels;
using Mwm.BeerFactoryV2.Uwp.Tips.Views;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using Serilog;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mwm.BeerFactoryV2.Uwp.Tips {
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication {
        public App() {
            InitializeComponent();
        }

        protected override void ConfigureContainer() {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            Container.RegisterType<ITemperatureControllerService, SerialUsbArduinoTemperatureControllerService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBackgroundTaskService, BackgroundTaskService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            Container.RegisterType<IBeerFactory, BeerFactory>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<BlankViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<SettingsViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<MainPhase>(new ContainerControlledLifetimeManager());


            Task.Run(() => {
                Container.Resolve<ITemperatureControllerService>().Run();
            });
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args) {
            await LaunchApplicationAsync(PageTokens.MainPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam) {
            Services.ThemeSelectorService.SetRequestedTheme();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            await Task.CompletedTask;
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args) {
            await Task.CompletedTask;
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args) {
            base.OnBackgroundActivated(args);
            CreateAndConfigureContainer();
            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args) {
            var backgroundTasks = Container.ResolveAll<IBackgroundTask>();

            await Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasksAsync(backgroundTasks);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            ViewModelLocationProvider.Register<ShellPage>(() => {
                return Container.Resolve<ShellViewModel>();
            });

            ViewModelLocationProvider.Register<SettingsPage>(() => {
                return Container.Resolve<SettingsViewModel>();
            });

            ViewModelLocationProvider.Register<BlankPage>(() => {
                return Container.Resolve<BlankViewModel>();
            });

            ViewModelLocationProvider.Register<MainPage>(() => {
                return Container.Resolve<MainPhase>();
            });



            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            //ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) => {
            //    string viewModelTypeName = "";
            //    if (viewType.Name != "MainPage") {
            //        var viewName = $"{viewType.Name.Replace("Page", "")}ViewModel";
            //        viewModelTypeName = $"Mwm.BeerFactoryV2.Uwp.Tips.ViewModels.{viewName}ViewModel, Mwm.BeerFactoryV2.Uwp.Tips";
            //    } else {
            //        var phaseName = $"{viewType.Name.Replace("Page", "")}Phase";
            //        viewModelTypeName = $"Mwm.BeerFactoryV2.Service.Phases.{phaseName}, Mwm.BeerFactoryV2.Service";
            //    }
            //    return Type.GetType(viewModelTypeName);
            //});

            //ViewModelLocationProvider.SetDefaultViewModelFactory((viewType) => {
            //    return Container.Resolve(viewType);
            //});


            await base.OnInitializeAsync(args);
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
