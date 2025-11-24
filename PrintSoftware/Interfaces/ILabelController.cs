using System.Data;
using PrintSoftware.Domain.Label;

namespace PrintSoftware.Interfaces;

public interface ILabelController
{
    Label? GetLabel(string labelName);
    void SetLabel(Label label);
    List<Label> GetAllLabels();
    Label GetCurrentLabel();
    List<LabelField> GetLabelFields();
    void UpdateLabelElementData(string name, string data);
    void UpdateLabelDataFromRow(DataRow row);
}