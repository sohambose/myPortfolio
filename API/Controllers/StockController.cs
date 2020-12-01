using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StockController : BaseAPIController
    {
        private readonly DataContext _context;
        public StockController(DataContext context)
        {
            this._context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Stock>>> GetAllStocks()
        {
            List<Stock> lstStocks = new List<Stock>();
            lstStocks = await _context.Stocks.ToListAsync();

            return lstStocks;
        }

        [HttpGet("{stockID}")]
        [AllowAnonymous]
        public async Task<ActionResult<Stock>> getStockByStockID(int stockID)
        {
            Stock stockObj = new Stock();
            stockObj = await _context.Stocks.SingleOrDefaultAsync(s => s.stockID == stockID);

            return stockObj;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<int>> SaveStock(Stock stockObjParam)
        {
            //-------------Use DTO Later---------------
            Stock stockObjSave = new Stock();

            stockObjSave.stockSymbol = stockObjParam.stockSymbol;
            stockObjSave.companyName = stockObjParam.companyName;
            stockObjSave.industry = stockObjParam.industry;
            stockObjSave.quantity = stockObjParam.quantity;
            //------------------------------------------
            Stock stockSaved = await _context.Stocks.SingleOrDefaultAsync(s => s.stockID == stockObjParam.stockID);
            if (stockSaved == null)
            {
                _context.Stocks.Add(stockObjSave);
            }
            else
            {
                stockSaved.stockSymbol = stockObjSave.stockSymbol;
                stockSaved.companyName = stockObjSave.companyName;
                stockSaved.industry = stockObjSave.industry;
                stockSaved.quantity = stockObjSave.quantity;
            }
            _context.SaveChanges();

            return stockObjSave.stockID;
        }
    }
}