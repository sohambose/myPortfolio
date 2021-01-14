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

    }
}