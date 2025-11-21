using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintSoftware.Interfaces;
using PrintSoftware.Services;

namespace PrintSoftware.Controller
{
    public class ExcelImportController : IExcelImportService
    {
        private readonly ExcelImportService _excelService;

        public ExcelImportController()
        {
            _excelService = new ExcelImportService();
        }

        public DataTable ImportExcelFile(string filePath)
        {
            return _excelService.ImportExcelFile(filePath);
        }

    }
}
