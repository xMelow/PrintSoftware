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
    private readonly ILabelController _labelController;
    
    public ObservableCollection<Label> Labels { get; } = new();
    public Label? SelectedLabel { get; set; }

    public ICommand SelectLabelCommand { get; }

    public event Action<Label?>? LabelSelected;

    public LabelSelectViewModel(ILabelController labelController)
    {
        _labelController = labelController ?? throw new ArgumentNullException(nameof(labelController));;
        LoadLabels();
        SelectLabelCommand = new RelayCommand(SelectLabel);
    }

    private void LoadLabels()
    {
        var labels = _labelController.GetAllLabels();
        
        Labels.Clear();
        foreach (var label in labels)
        {
            Labels.Add(label);
        }
    }

    private void SelectLabel()
    {
        _labelController.SetLabel(SelectedLabel);
        LabelSelected?.Invoke(SelectedLabel);
    }
}
