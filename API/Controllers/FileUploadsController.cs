using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class FileUploadsController : BaseAPIController
    {
        private readonly DataContext _context;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(FileUploadsController));
        public FileUploadsController(DataContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UploadFile()
        {
            string filePath = string.Empty;
            int stockID = -1;
            IFormFile file = Request.Form.Files[0];

            if (!string.IsNullOrEmpty(Request.Form["stockID"]))
                stockID = int.Parse(Request.Form["stockID"].ToString());

            if (file.Length > 0)
            {
                filePath = @"D:\Projects\PersonalPortfolioManagement\myPortfolio\API\Files\" + file.FileName.ToString() + "_" + DateTime.Now.ToFileTime();
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }

            SaveCSVData(filePath, stockID);
            return Ok();
        }

        private void SaveCSVData(string filePath, int stockID)
        {
            try
            {
                TextReader reader = new StreamReader(filePath);
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                IEnumerable<StockFundamentalAttributeCSV> lstrecords = csvReader.GetRecords<StockFundamentalAttributeCSV>();

                _context.StockFundamentalAttributes.RemoveRange(_context.StockFundamentalAttributes.Where(sfa => sfa.stockID == stockID));
                _context.SaveChanges();

                List<StockFundamentalAttributes> lstSFA = new List<StockFundamentalAttributes>();


                foreach (StockFundamentalAttributeCSV csvitem in lstrecords)
                {
                    StockFundamentalAttributes sfaObj = new StockFundamentalAttributes();

                    sfaObj.stockID = stockID;

                    sfaObj.Statement = csvitem.Statement;
                    sfaObj.Head = csvitem.Head;

                    sfaObj.Y0 = string.IsNullOrEmpty(csvitem.Y0) ? "0" : csvitem.Y0;
                    sfaObj.Y1 = string.IsNullOrEmpty(csvitem.Y1) ? "0" : csvitem.Y1;
                    sfaObj.Y2 = string.IsNullOrEmpty(csvitem.Y2) ? "0" : csvitem.Y2;
                    sfaObj.Y3 = string.IsNullOrEmpty(csvitem.Y3) ? "0" : csvitem.Y3;
                    sfaObj.Y4 = string.IsNullOrEmpty(csvitem.Y4) ? "0" : csvitem.Y4;
                    sfaObj.Y5 = string.IsNullOrEmpty(csvitem.Y5) ? "0" : csvitem.Y5;
                    sfaObj.Y6 = string.IsNullOrEmpty(csvitem.Y6) ? "0" : csvitem.Y6;
                    sfaObj.Y7 = string.IsNullOrEmpty(csvitem.Y7) ? "0" : csvitem.Y7;
                    sfaObj.Y8 = string.IsNullOrEmpty(csvitem.Y8) ? "0" : csvitem.Y8;
                    sfaObj.Y9 = string.IsNullOrEmpty(csvitem.Y9) ? "0" : csvitem.Y9;

                    sfaObj.RecordTimeStamp = DateTime.Now;

                    /* if (sfaObj.Statement.ToUpper().Equals("PL"))
                        processPLData(sfaObj);

                    if (sfaObj.Statement.ToUpper().Equals("BALANCESHEET"))
                        processPLData(sfaObj);

                    if (sfaObj.Statement.ToUpper().Equals("BALANCESHEET"))
                        processPLData(sfaObj); */
                    processData(sfaObj);

                    lstSFA.Add(sfaObj);
                }

                _context.StockFundamentalAttributes.AddRange(lstSFA);
                _context.SaveChanges();

                //processPLData();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void processData(StockFundamentalAttributes sfa)
        {
            log.Debug("Processing " + sfa.Head);
            if (sfa.Head.ToUpper().Equals("GROSSPROFITMARGIN") || sfa.Head.ToUpper().Equals("NETPROFITPERCENTAGE")
                || sfa.Head.ToUpper().Equals("INVENTORYTURNOVER") || sfa.Head.ToUpper().Equals("ROE")
                || sfa.Head.ToUpper().Equals("EBTMARGIN") || sfa.Head.ToUpper().Equals("PATMARGIN")
                || sfa.Head.ToUpper().Equals("ROE") || sfa.Head.ToUpper().Equals("ROA")
                || sfa.Head.ToUpper().Equals("ROCE") || sfa.Head.ToUpper().Equals("INTERESTCOVERAGERATIO")
                || sfa.Head.ToUpper().Equals("DEBTTOEQUITYRATIO") || sfa.Head.ToUpper().Equals("FINANCIALLEVERAGERATIO")
                || sfa.Head.ToUpper().Equals("FIXEDASSETSTURNOVER") || sfa.Head.ToUpper().Equals("TOTALASSETSTURNOVERRATIO")
                || sfa.Head.ToUpper().Equals("INVENTORYTURNOVERRATIO") || sfa.Head.ToUpper().Equals("INVENTORYNUMBEROFDAYS")
                || sfa.Head.ToUpper().Equals("RECEIVABLETURNOVERRATIO") || sfa.Head.ToUpper().Equals("DAYSSALESOUTSTANDING")
               )
            {
                decimal sum = decimal.Parse(sfa.Y0) + decimal.Parse(sfa.Y1) + decimal.Parse(sfa.Y2) + decimal.Parse(sfa.Y3)
                 + decimal.Parse(sfa.Y4) + decimal.Parse(sfa.Y5) + decimal.Parse(sfa.Y6) + decimal.Parse(sfa.Y7) +
                 decimal.Parse(sfa.Y8) + decimal.Parse(sfa.Y9);

                decimal average = sum / 9;

                sfa.observationValueType = "Average";
                sfa.observationValue = average;
            }
            else
            {
                decimal EndingValue = decimal.Parse(sfa.Y0);
                decimal BeginningValue = decimal.Parse(sfa.Y9);
                int years = 9;
                double Value = 0.0;

                if (EndingValue > 0 && BeginningValue > 0)
                    Value = (Math.Pow((Convert.ToDouble(EndingValue) / Convert.ToDouble(BeginningValue)), (double)1 / (double)years) - 1) * 100;

                sfa.observationValueType = "CAGR";
                sfa.observationValue = Convert.ToDecimal(Value);
            }
        }


        private void processBalanceSheetData(StockFundamentalAttributes sfa)
        {
            if (sfa.Head.ToUpper().Equals("INVENTORYTURNOVER") || sfa.Head.ToUpper().Equals("ROE"))
            {
                decimal sum = decimal.Parse(sfa.Y0) + decimal.Parse(sfa.Y1) + decimal.Parse(sfa.Y2) + decimal.Parse(sfa.Y3)
                 + decimal.Parse(sfa.Y4) + decimal.Parse(sfa.Y5) + decimal.Parse(sfa.Y6) + decimal.Parse(sfa.Y7) +
                 decimal.Parse(sfa.Y8) + decimal.Parse(sfa.Y9);

                decimal average = sum / 9;

                sfa.observationValueType = "Average";
                sfa.observationValue = average;
            }
            else
            {
                decimal EndingValue = decimal.Parse(sfa.Y0);
                decimal BeginningValue = decimal.Parse(sfa.Y9);
                int years = 9;

                double Value = (Math.Pow((Convert.ToDouble(EndingValue) / Convert.ToDouble(BeginningValue)), (double)1 / (double)years) - 1) * 100;

                sfa.observationValueType = "CAGR";
                sfa.observationValue = Convert.ToDecimal(Value);
            }
        }


        #region <Commented>
        /* //----This uses EPPlus-------
         * public DataTable ReadExcelToTable(string filePath) 
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                DataTable dtExcel = new DataTable();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(fileInfo);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // get number of rows and columns in the sheet
                int rows = worksheet.Dimension.Rows; // 20
                int columns = worksheet.Dimension.Columns; // 7

                for (int j = 1; j <= columns; j++)
                {
                    DataColumn dc = new DataColumn();
                    dc.DataType = typeof(string);
                    dc.ColumnName = j.ToString();

                    dtExcel.Columns.Add(dc);
                }

                // loop through the worksheet rows and columns
                for (int i = 1; i <= rows; i++)
                {
                    DataRow dataRow = dtExcel.NewRow();
                    for (int j = 1; j <= columns; j++)
                    {

                        string content = worksheet.Cells[i, j].Value.ToString();
                        dataRow[j - 1] = content;                       
                    }
                    dtExcel.Rows.Add(dataRow);
                }
                return dtExcel;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }*/
        #endregion


    }
}