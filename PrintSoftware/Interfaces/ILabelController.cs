using System.Data;
using PrintSoftware.Domain.Label;

namespace PrintSoftware.Interfaces;

public interface ILabelController
{
    Label? GetLabel(string labelName);
    Label GetCurrentLabel();
    void UpdateLabelElementData(string name, string data);
    void UpdateLabelDataFromRow(DataRow row);
}