namespace PrintSoftware.Interfaces;

public interface IWindowService
{
    string? ShowOpenExcelDialog();
    string? ShowLabelSelectDialog();
    void ShowSettingsDialog();
}