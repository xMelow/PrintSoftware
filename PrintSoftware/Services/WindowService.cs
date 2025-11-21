using Microsoft.Win32;
using PrintSoftware.Domain.Label;
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
    
    private readonly ILabelController _labelController;
    
    public WindowService() { }
    
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
        var vm = new LabelSelectViewModel(_labelController);
        var window = new LabelSelectWindow();

        Label? selected = null;
        vm.LabelSelected += label => selected = label;

        window.ShowDialog();
        return selected;
    }

    public void ShowSettingsScreen()
    {
        var window = new Views.SettingsWindow();
        window.ShowDialog();
    }
}