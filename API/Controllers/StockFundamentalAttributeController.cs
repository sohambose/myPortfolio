using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BLL;
using API.Data;
using API.DTOS;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StockFundamentalAttributeController : BaseAPIController
    {
        private readonly DataContext _context;
        public StockFundamentalAttributeController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("{stockID}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<StockFundamentalAttributes>>> GetStockFundamentalAttribute(int stockID)
        {
            List<StockFundamentalAttributes> lstStockFADB = new List<StockFundamentalAttributes>();

            lstStockFADB = await _context.StockFundamentalAttributes.ToListAsync<StockFundamentalAttributes>();
            lstStockFADB = lstStockFADB.FindAll(sfa => sfa.stockID == stockID);

            List<StockFundamentalAttributes> retlstStockFA = new List<StockFundamentalAttributes>();

            StockFundamentalBLL stockFundamentalBLL = new StockFundamentalBLL();
            stockFundamentalBLL.SortFundamentalDataByStmtType(lstStockFADB, retlstStockFA);

            return retlstStockFA;
        }

        [HttpGet("compare/{SelectedStocks}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StockScoreDTO>>> CompareStockFundamentals(string SelectedStocks)
        {
            //1->---Delete and Insert in Stock Comparison Table for Detailed Reporting-
            _context.StockComparisonScores.RemoveRange(_context.StockComparisonScores);
            _context.SaveChanges();

            List<StockFundamentalAttributes> lstStockFADB = new List<StockFundamentalAttributes>();
            lstStockFADB = await _context.StockFundamentalAttributes.Include("stock").ToListAsync<StockFundamentalAttributes>();

            List<StockComparisonScores> lstScoreSave = new List<StockComparisonScores>();
            lstScoreSave = StockFundamentalBLL.GetInstance().GetStockComparisonFromSFA(lstStockFADB);

            _context.StockComparisonScores.AddRange(lstScoreSave);
            _context.SaveChanges();


            //2->Return Score Report as JSON to caller for display:
            IEnumerable<StockScoreDTO> ScoreReportSummary = new List<StockScoreDTO>();
            List<Stock> lstAllStocks = _context.Stocks.ToList<Stock>();
            ScoreReportSummary = StockFundamentalBLL.GetInstance().GetScoreReportSummary(lstScoreSave, lstAllStocks);

            return ScoreReportSummary.ToList<StockScoreDTO>();
        }

        [HttpGet("compareDetails")]
        [AllowAnonymous]
        public async Task<ActionResult<List<StockComparisonDetailsDTO>>> GetStockComparisonDetails()
        {
            List<StockComparisonScores> lstStockCompareDB = new List<StockComparisonScores>();
            List<StockComparisonDetailsDTO> retlstStockCompare = new List<StockComparisonDetailsDTO>();

            lstStockCompareDB = await _context.StockComparisonScores.ToListAsync<StockComparisonScores>();
            List<Stock> lstAllStocks = await _context.Stocks.ToListAsync<Stock>();

            retlstStockCompare = StockFundamentalBLL.GetInstance().GenerateDetailedComparison(lstStockCompareDB, lstAllStocks);

            return retlstStockCompare;
        }
    }
}