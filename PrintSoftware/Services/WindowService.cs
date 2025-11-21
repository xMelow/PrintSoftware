using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using PrintSoftware.Controller;
using PrintSoftware.Domain.Label;
using PrintSoftware.Interfaces;
using PrintSoftware.ViewModels;
using PrintSoftware.Views;

namespace PrintSoftware.Services;

public class WindowService : IWindowService
{
    
    //TODO:
    // 1. update function names
    
    private readonly ILabelController _labelController;
    private readonly IPrintController _printController;
    
    public WindowService() {}
    
    public WindowService(
        ILabelController labelController,
        IPrintController printController
    ) {
        _labelController = labelController ?? throw new ArgumentNullException(nameof(labelController));
        _printController = printController ?? throw new ArgumentNullException(nameof(printController));
    }
    
    public string? OpenExcelDialog()
    {
        var dialog = new OpenFileDialog()
        {
            Filter = "Excel Files|*.xlsx; *.xls",
        };
        
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public Label? ShowLabelSelectScreen()
    {
        var vm = App.HostContainer.Services.GetRequiredService<LabelSelectViewModel>();
        var window = App.HostContainer.Services.GetRequiredService<LabelSelectWindow>();
        window.DataContext = vm;

        Label? selected = null;
        vm.LabelSelected += label =>
        {
            selected = label;
            window.DialogResult = true;
        };

        window.ShowDialog();
        return selected;
    }

    public void ShowSettingsScreen()
    {
        var vm = new SettingsViewModel(_printController);
        var window = new SettingsWindow
        {
            DataContext = vm,
            Owner = System.Windows.Application.Current.MainWindow
        };

        window.ShowDialog();
    }
}