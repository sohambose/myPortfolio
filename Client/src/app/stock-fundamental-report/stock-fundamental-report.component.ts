import { StockService } from './../_Services/Stock.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-stock-fundamental-report',
  templateUrl: './stock-fundamental-report.component.html',
  styleUrls: ['./stock-fundamental-report.component.css']
})
export class StockFundamentalReportComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private stockService: StockService) { }

  selectedStockID: number;
  stock: any;
  stockSymbol: string;
  stockCompanyName: string;
  stockIndustry: string;

  stockPL: any[];
  stockBalanceSheet: any[];
  stockCashFlow: any[];
  stockRatios: any[];



  ngOnInit(): void {
    this.activatedRoute.params.subscribe(r => {
      this.selectedStockID = r['stockID'];
      this.loadStockDetails();
      this.loadStockFundamentals();
    })
  }

  loadStockDetails() {
    this.stockService.getStockByID(this.selectedStockID).subscribe(res => {
      this.stock = res;
      this.stockSymbol = this.stock.stockSymbol;
      this.stockCompanyName = this.stock.companyName;
      this.stockIndustry = this.stock.industry;
    });
  }

  loadStockFundamentals() {
    this.stockService.getStockFundamentalAttributes(this.selectedStockID).subscribe(res => {
      this.stockPL = res.filter(r => r.statement == 'PL');
      this.stockBalanceSheet = res.filter(r => r.statement == 'BALANCESHEET');
      this.stockCashFlow = res.filter(r => r.statement == 'CASHFLOW');
      this.stockRatios = res.filter(r => (r.statement == 'LeverageRatio' || r.statement=='OperatingRatio ' || r.statement=='ProfitabilityRatio'));
    });
  }
}
