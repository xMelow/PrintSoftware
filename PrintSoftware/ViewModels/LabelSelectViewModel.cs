using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PrintSoftware.Domain.Label;
using PrintSoftware.Interfaces;
using PrintSoftware.ViewModels;
using PrintSoftware.ViewModels.Commands;

namespace PrintSoftware.ViewModels;

public class LabelSelectViewModel : BaseViewModel
{
    //TODO: UPDATE THIS FILE 
    
    public ObservableCollection<string> Labels { get; } = new();
    public Label? SelectedLabel { get; set; }

    public ICommand SelectLabelCommand { get; }

    public event Action<Label?>? LabelSelected;

    public LabelSelectViewModel()
    {
        LoadLabels();
        SelectLabelCommand = new RelayCommand(SelectLabel);
    }

    private void LoadLabels()
    {
        var labelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Labels");

        if (!Directory.Exists(labelsFolder))
        {
            // Optionally, raise an error event instead of using MessageBox
            LabelSelected?.Invoke(null);
            return;
        }

        var labelFiles = Directory.GetFiles(labelsFolder)
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        Labels.Clear();
        foreach (var label in labelFiles)
            Labels.Add(label);
    }

    private void SelectLabel()
    {
        LabelSelected?.Invoke(SelectedLabel);
    }
}
