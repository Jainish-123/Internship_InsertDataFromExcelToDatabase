using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classADO_11
{
    public class test
    {
        private string sqlConStr;
        private SqlConnection sqlCon;
        private DataTable Exceldt;

        public void sqlconnection()
        {
            sqlConStr = @"Data Source = DESKTOP-TA8BLLL\SQLEXPRESS; Initial Catalog = employee; Integrated Security = true;";
            sqlCon = new SqlConnection(sqlConStr);
        }

        public void insertDataIntoDatabase_2(string excelFilePath)
        {
            using (XLWorkbook workBook = new XLWorkbook(excelFilePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet("Sheet1");

                Exceldt = new DataTable();

                bool firstRow = true;

                foreach (IXLRow row in workSheet.Rows())
                {
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            Exceldt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        Exceldt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            Exceldt.Rows[Exceldt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                sqlconnection();

                sqlCon.Open();

                for (int i = 0; i < Exceldt.Rows.Count; i++)
                {
                    //SqlDataAdapter adp = new SqlDataAdapter("" +
                    //    "insert into empData" +
                    //    " (emp_name, emp_mo_no, emp_city) " +
                    //    "values" +
                    //    "('" + Exceldt.Rows[i][1] + "','" + Exceldt.Rows[i][2] + "', '" + Exceldt.Rows[i][3] + "')", sqlCon);

                    SqlDataAdapter adp = new SqlDataAdapter($"spEmployee '{Exceldt.Rows[i][1]}', '{Exceldt.Rows[i][2]}', '{Exceldt.Rows[i][3]}' ", sqlCon);

                    adp.SelectCommand.ExecuteNonQuery();
                }

                sqlCon.Close();
            }
            
        }
    }
}
