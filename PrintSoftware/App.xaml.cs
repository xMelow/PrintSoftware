using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrintSoftware.Controller;
using PrintSoftware.Interfaces;
using PrintSoftware.Services;
using PrintSoftware.ViewModels;
using PrintSoftware.Views;

namespace PrintSoftware
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost HostContainer { get; private set; } = null!;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            HostContainer = CreateHostBuilder().Build();
            HostContainer.Start();

            var mainWindow = HostContainer.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IPrintController, PrintController>();
                    services.AddSingleton<ILabelController, LabelController>();
                    services.AddSingleton<ILabelPreviewController, LabelPreviewController>();
                    services.AddSingleton<IExcelImportService, ExcelImportController>();
                    services.AddSingleton<IWindowService, WindowService>();

                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<LabelSelectViewModel>();
                    services.AddTransient<SettingsViewModel>();

                    services.AddTransient<MainWindow>();
                    services.AddTransient<LabelSelectWindow>();
                    services.AddTransient<SettingsWindow>();
                });
    }
}