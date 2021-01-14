using System;
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
            Stock stockObjSave = new Stock();
            bool isAddNew = true;
            stockObjSave = await _context.Stocks.SingleOrDefaultAsync(s => s.stockID == stockObjParam.stockID);
            if (stockObjSave == null)
            {
                isAddNew = true;
                stockObjSave = new Stock();
            }
            else
            {
                isAddNew = false;
            }

            stockObjSave.stockSymbol = stockObjParam.stockSymbol;
            stockObjSave.companyName = stockObjParam.companyName;
            stockObjSave.industry = stockObjParam.industry;
            stockObjSave.quantity = stockObjParam.quantity;

            if (isAddNew)
                _context.Stocks.Add(stockObjSave);

            _context.SaveChanges();

            return stockObjSave.stockID;
        }

        [HttpDelete("{stockID}")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> DeleteStock(int stockID)
        {
            Stock stockObj = new Stock();
            stockObj = await _context.Stocks.SingleOrDefaultAsync(s => s.stockID == stockID);

            if (stockObj != null)
            {
                _context.Stocks.Remove(stockObj);
                _context.SaveChanges();
            }
            return 1;
        }
    }
}