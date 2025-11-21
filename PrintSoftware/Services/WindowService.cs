using Microsoft.Win32;
using PrintSoftware.Interfaces;
using PrintSoftware.ViewModels;
using PrintSoftware.Views;

namespace PrintSoftware.Services;

public class WindowService : IWindowService
{
    
    //TODO:
    // 1. update function names
    // 2. don't return null in ShowLabelSelectDialog
    // 3. labelSelectWindow needs label controller
    
    // this is a comment 
    
    private readonly ILabelController _labelController;

    public WindowService(ILabelController labelController)
    {
        _labelController = labelController;
    }
    
    public string? ShowOpenExcelDialog()
    {
        var dialog = new OpenFileDialog()
        {
            Filter = "Excel Files|*.xlsx; *.xls",
        };
        
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? ShowLabelSelectDialog()
    {
        var vm = new LabelSelectViewModel(_labelController);
        var window = new LabelSelectWindow(vm);

        return window.ShowDialog() == true
            ? vm.SelectedLabel
            : null;
    }

    public void ShowSettingsDialog()
    {
        var window = new Views.SettingsWindow();
        window.ShowDialog();
    }
}