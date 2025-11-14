using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintSoftware.Services;

namespace PrintSoftware.Controller
{
    public class ExcelImportController
    {
        private readonly ExcelImportService _excelService;

        public ExcelImportController()
        {
            _excelService = new ExcelImportService();
        }

        public DataTable ImportExcel(string filePath)
        {
            return _excelService.ImportExcel(filePath);
        }

    }
}
