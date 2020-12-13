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
                    sfaObj.Y0 = csvitem.Y0;
                    sfaObj.Y1 = csvitem.Y1;
                    sfaObj.Y2 = csvitem.Y2;
                    sfaObj.Y3 = csvitem.Y3;
                    sfaObj.Y4 = csvitem.Y4;
                    sfaObj.Y5 = csvitem.Y5;
                    sfaObj.Y6 = csvitem.Y6;
                    sfaObj.Y7 = csvitem.Y7;
                    sfaObj.Y8 = csvitem.Y8;
                    sfaObj.Y9 = csvitem.Y9;

                    sfaObj.RecordTimeStamp = DateTime.Now;

                    lstSFA.Add(sfaObj);
                }

                _context.StockFundamentalAttributes.AddRange(lstSFA);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
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