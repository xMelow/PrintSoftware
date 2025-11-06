using SimpleProject.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject.Controller
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
