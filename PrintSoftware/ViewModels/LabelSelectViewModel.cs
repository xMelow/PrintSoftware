using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using PrintSoftware.Domain.Label;
using PrintSoftware.Interfaces;
using PrintSoftware.ViewModels;
using PrintSoftware.ViewModels.Commands;

public class LabelSelectViewModel : BaseViewModel
{
    private readonly ILabelController _labelController;
    public ObservableCollection<string> Labels { get; } = new();

    private string? _selectedLabel;
    public string? SelectedLabel
    {
        get => _selectedLabel;
        set
        {
            if (_selectedLabel == value) return;
            _selectedLabel = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectLabelCommand { get; }

    public event Action<string?>? LabelSelected;

    public LabelSelectViewModel(ILabelController labelController)
    {
        _labelController = labelController;
        SelectLabelCommand = new RelayCommand(SelectLabel);

        LoadLabels();
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
        if (string.IsNullOrWhiteSpace(SelectedLabel))
        {
            LabelSelected?.Invoke(null);
            return;
        }

        // Notify subscribers (usually the WindowService) that a label has been selected
        LabelSelected?.Invoke(SelectedLabel);
    }
}
