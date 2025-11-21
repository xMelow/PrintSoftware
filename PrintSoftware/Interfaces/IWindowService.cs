
using PrintSoftware.Domain.Label;

namespace PrintSoftware.Interfaces;

public interface IWindowService
{
    string? OpenExcelDialog();
    Label? ShowLabelSelectScreen();
    void ShowSettingsScreen();
}