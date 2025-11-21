using System.Data;

namespace PrintSoftware.Interfaces;

public interface IExcelImportService
{
    DataTable ImportExcelFile(string path);
}