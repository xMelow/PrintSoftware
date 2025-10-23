using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SimpleProject.Services
{
    public class ExcelImportService
    {
        public DataTable ImportExcel(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            });

            var table = result.Tables[0];
            Console.WriteLine($"Rows: {table.Rows.Count}, Cols: {table.Columns.Count}");

            return result.Tables[0];
        }
    }
}
