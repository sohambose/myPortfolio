using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.BLL;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class FileUploadsController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(FileUploadsController));
        public FileUploadsController(DataContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UploadFile()
        {
            string filePath = string.Empty;
            int stockID = -1;
            int uploadType = -1;

            IFormFile file = Request.Form.Files[0];

            if (!string.IsNullOrEmpty(Request.Form["stockID"]))
                stockID = int.Parse(Request.Form["stockID"].ToString());

            if (!string.IsNullOrEmpty(Request.Form["uploadType"]))
                uploadType = int.Parse(Request.Form["uploadType"].ToString());

            if (file.Length > 0)
            {
                string PhysicalAppPath = _configuration["AppSettings:PhysicalAppPath"];
                string userFiles = _configuration["AppSettings:userUploadedFiles"];
                string fileNamingConv = DateTime.Now.ToFileTime() + "_" + file.FileName.ToString();

                //filePath = @"D:\Projects\PersonalPortfolioManagement\myPortfolio\API\Files\" + file.FileName.ToString() + "_" + DateTime.Now.ToFileTime();
                filePath = Path.Combine(PhysicalAppPath, userFiles, fileNamingConv);

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
            }

            if (uploadType == 1)
            {
                //---Delete stock Data Before inserting--
                _context.StockFundamentalAttributes.RemoveRange(_context.StockFundamentalAttributes.Where(sfa => sfa.stockID == stockID));
                _context.SaveChanges();

                List<StockFundamentalAttributes> lstSFA = new List<StockFundamentalAttributes>();
                CSVDataProcessor csvDataProcessor = new CSVDataProcessor();
                lstSFA = csvDataProcessor.ProcessYearlyCSVData(filePath, stockID);

                _context.StockFundamentalAttributes.AddRange(lstSFA);
                _context.SaveChanges();
            }
            else if (uploadType == 2)
            {
                _context.StockQuarterlyData.RemoveRange(_context.StockQuarterlyData.Where(sfa => sfa.stockID == stockID));
                _context.SaveChanges();

                List<StockQuarterlyData> lstSQD = new List<StockQuarterlyData>();
                CSVDataProcessor csvDataProcessor = new CSVDataProcessor();
                lstSQD = csvDataProcessor.ProcessQuarterlyCSVData(filePath, stockID);

                _context.StockQuarterlyData.AddRange(lstSQD);
                _context.SaveChanges();
            }

            return Ok();
        }

        /*  private void SaveCSVDataYearly(string filePath, int stockID)
         {
             try
             {
                 int roundingPlaces = 2;
                 TextReader reader = new StreamReader(filePath);
                 var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                 IEnumerable<StockFundamentalAttributeCSV> lstrecords = csvReader.GetRecords<StockFundamentalAttributeCSV>();

                 //_context.StockFundamentalAttributes.RemoveRange(_context.StockFundamentalAttributes.Where(sfa => sfa.stockID == stockID));
                 //_context.SaveChanges();

                 List<StockFundamentalAttributes> lstSFA = new List<StockFundamentalAttributes>();


                 foreach (StockFundamentalAttributeCSV csvitem in lstrecords)
                 {
                     StockFundamentalAttributes sfaObj = new StockFundamentalAttributes();

                     sfaObj.stockID = stockID;

                     sfaObj.Statement = csvitem.Statement;
                     sfaObj.Head = csvitem.Head;

                     sfaObj.Y0 = string.IsNullOrEmpty(csvitem.Y0) ? 0 : decimal.Round(decimal.Parse(csvitem.Y0), roundingPlaces);
                     sfaObj.Y1 = string.IsNullOrEmpty(csvitem.Y1) ? 0 : decimal.Round(decimal.Parse(csvitem.Y1), roundingPlaces);
                     sfaObj.Y2 = string.IsNullOrEmpty(csvitem.Y2) ? 0 : decimal.Round(decimal.Parse(csvitem.Y2), roundingPlaces);
                     sfaObj.Y3 = string.IsNullOrEmpty(csvitem.Y3) ? 0 : decimal.Round(decimal.Parse(csvitem.Y3), roundingPlaces);
                     sfaObj.Y4 = string.IsNullOrEmpty(csvitem.Y4) ? 0 : decimal.Round(decimal.Parse(csvitem.Y4), roundingPlaces);
                     sfaObj.Y5 = string.IsNullOrEmpty(csvitem.Y5) ? 0 : decimal.Round(decimal.Parse(csvitem.Y5), roundingPlaces);
                     sfaObj.Y6 = string.IsNullOrEmpty(csvitem.Y6) ? 0 : decimal.Round(decimal.Parse(csvitem.Y6), roundingPlaces);
                     sfaObj.Y7 = string.IsNullOrEmpty(csvitem.Y7) ? 0 : decimal.Round(decimal.Parse(csvitem.Y7), roundingPlaces);
                     sfaObj.Y8 = string.IsNullOrEmpty(csvitem.Y8) ? 0 : decimal.Round(decimal.Parse(csvitem.Y8), roundingPlaces);
                     sfaObj.Y9 = string.IsNullOrEmpty(csvitem.Y9) ? 0 : decimal.Round(decimal.Parse(csvitem.Y9), roundingPlaces);

                     sfaObj.RecordTimeStamp = DateTime.Now;
                     CalculateValues(sfaObj);

                     lstSFA.Add(sfaObj);
                 }

                 _context.StockFundamentalAttributes.AddRange(lstSFA);
                 _context.SaveChanges();
             }
             catch (Exception ex)
             {
                 log.Error(ex.Message);
             }
         } */

        /* private void CalculateValues(StockFundamentalAttributes sfa)
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
                decimal average = (sfa.Y0 + sfa.Y1 + sfa.Y2 + sfa.Y3 + sfa.Y4 + sfa.Y5 + sfa.Y6 + sfa.Y7 + sfa.Y8 + sfa.Y9) / 9;
                sfa.observationValueType = "Average";
                sfa.observationValue = average;
            }
            else
            {
                decimal EndingValue = sfa.Y0;
                decimal BeginningValue = sfa.Y9;
                int years = 9;
                double Value = 0.0;

                if (EndingValue > 0 && BeginningValue > 0)
                    Value = (Math.Pow((Convert.ToDouble(EndingValue) / Convert.ToDouble(BeginningValue)), (double)1 / (double)years) - 1) * 100;

                sfa.observationValueType = "CAGR";
                sfa.observationValue = Convert.ToDecimal(Value);
            }
        } */

        /* private void SaveCSVDataQuarterly(string filePath, int stockID)
        {
            try
            {
                int roundingPlaces = 2;

                TextReader reader = new StreamReader(filePath);
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                IEnumerable<StockQuarterlyDataCSV> lstQtrlyrecords = csvReader.GetRecords<StockQuarterlyDataCSV>();

                _context.StockQuarterlyData.RemoveRange(_context.StockQuarterlyData.Where(sfa => sfa.stockID == stockID));
                _context.SaveChanges();

                List<StockQuarterlyData> lstSQD = new List<StockQuarterlyData>();

                foreach (StockQuarterlyDataCSV csvitem in lstQtrlyrecords)
                {
                    StockQuarterlyData sqdObj = new StockQuarterlyData();

                    sqdObj.stockID = stockID;
                    sqdObj.Narration = csvitem.Narration;

                    sqdObj.Q0 = string.IsNullOrEmpty(csvitem.Q0) ? 0 : decimal.Round(decimal.Parse(csvitem.Q0), roundingPlaces);
                    sqdObj.Q1 = string.IsNullOrEmpty(csvitem.Q1) ? 0 : decimal.Round(decimal.Parse(csvitem.Q1), roundingPlaces);
                    sqdObj.Q2 = string.IsNullOrEmpty(csvitem.Q2) ? 0 : decimal.Round(decimal.Parse(csvitem.Q2), roundingPlaces);
                    sqdObj.Q3 = string.IsNullOrEmpty(csvitem.Q3) ? 0 : decimal.Round(decimal.Parse(csvitem.Q3), roundingPlaces);
                    sqdObj.Q4 = string.IsNullOrEmpty(csvitem.Q4) ? 0 : decimal.Round(decimal.Parse(csvitem.Q4), roundingPlaces);
                    sqdObj.Q5 = string.IsNullOrEmpty(csvitem.Q5) ? 0 : decimal.Round(decimal.Parse(csvitem.Q5), roundingPlaces);
                    sqdObj.Q6 = string.IsNullOrEmpty(csvitem.Q6) ? 0 : decimal.Round(decimal.Parse(csvitem.Q6), roundingPlaces);
                    sqdObj.Q7 = string.IsNullOrEmpty(csvitem.Q7) ? 0 : decimal.Round(decimal.Parse(csvitem.Q7), roundingPlaces);
                    sqdObj.Q8 = string.IsNullOrEmpty(csvitem.Q8) ? 0 : decimal.Round(decimal.Parse(csvitem.Q8), roundingPlaces);
                    sqdObj.Q9 = string.IsNullOrEmpty(csvitem.Q9) ? 0 : decimal.Round(decimal.Parse(csvitem.Q9), roundingPlaces);

                    sqdObj.RecordTimeStamp = DateTime.Now;

                    //-----Calculate CAGR and insert---
                    decimal EndingValue = sqdObj.Q0;
                    decimal BeginningValue = sqdObj.Q9;
                    int quarters = 9;
                    double Value = 0.0;

                    if (EndingValue > 0 && BeginningValue > 0)
                        Value = (Math.Pow((Convert.ToDouble(EndingValue) / Convert.ToDouble(BeginningValue)), (double)1 / (double)quarters) - 1) * 100;

                    sqdObj.observationValueType = "CAGR";
                    sqdObj.observationValue = Convert.ToDecimal(Value);
                    //----------------------------------

                    lstSQD.Add(sqdObj);
                }

                //----------Insert Row For EBIT----------------------------------
                StockQuarterlyData sqdEBIT = new StockQuarterlyData();
                sqdEBIT.stockID = stockID;
                sqdEBIT.Narration = "EBIT";
                sqdEBIT.Q9 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q9 +
                           lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q9 +
                           lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q9 +
                           lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q9;

                sqdEBIT.Q8 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q8 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q8 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q8 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q8;

                sqdEBIT.Q7 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q7 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q7 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q7 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q7;

                sqdEBIT.Q6 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q6 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q6 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q6 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q6;

                sqdEBIT.Q5 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q5 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q5 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q5 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q5;

                sqdEBIT.Q4 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q4 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q4 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q4 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q4;

                sqdEBIT.Q3 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q3 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q3 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q3 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q3;

                sqdEBIT.Q2 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q2 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q2 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q2 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q2;

                sqdEBIT.Q1 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q1 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q1 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q1 +
               lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q1;

                sqdEBIT.Q0 = lstSQD.Find(q => q.Narration.ToUpper().Equals("OPERATINGPROFIT")).Q0 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("DEPRECIATION")).Q0 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("INTEREST")).Q0 +
                lstSQD.Find(q => q.Narration.ToUpper().Equals("TAX")).Q0;


                //-----Calculate CAGR and insert---
                decimal EndingValueEBIT = sqdEBIT.Q0;
                decimal BeginningValueEBIT = sqdEBIT.Q9;
                int quartersEBIT = 9;
                double ValueEBIT = 0.0;

                if (EndingValueEBIT > 0 && BeginningValueEBIT > 0)
                    ValueEBIT = (Math.Pow((Convert.ToDouble(EndingValueEBIT) / Convert.ToDouble(BeginningValueEBIT)), (double)1 / (double)quartersEBIT) - 1) * 100;

                sqdEBIT.observationValueType = "CAGR";
                sqdEBIT.observationValue = Convert.ToDecimal(ValueEBIT);
                //----------------------------------

                lstSQD.Add(sqdEBIT);

                //---------------------------------------------------------------

                _context.StockQuarterlyData.AddRange(lstSQD);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

 */
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