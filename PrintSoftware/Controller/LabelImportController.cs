using PrintSoftware.Domain.Label;

namespace PrintSoftware.Controller;

public class LabelImportController
{
    private readonly LabelImportController _labelImportController;

    public LabelImportController()
    {
        _labelImportController = new LabelImportController();
    }

    public Label ImportLabel()
    {
        return _labelImportController.ImportLabel();
    }
}