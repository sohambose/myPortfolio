import { StockService } from './../_Services/Stock.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-stock-comparison-report',
  templateUrl: './stock-comparison-report.component.html',
  styleUrls: ['./stock-comparison-report.component.css']
})
export class StockComparisonReportComponent implements OnInit {

  arrStockListStr: any;
  isShowDetailedReport: boolean = false;
  showHideDetailedReportCaption: string = 'Show Detailed Report';

  stockReport: any;

  isNoDataFound: boolean;
  stockPL: any[];
  stockBalanceSheet: any[];
  stockCashFlow: any[];
  stockRatios: any[];

  constructor(private activatedroute: ActivatedRoute, private stockService: StockService) { }

  ngOnInit(): void {
    console.log('In Component');
    this.activatedroute.params.subscribe(res => {
      this.arrStockListStr = res.stockIDList;
      console.log(this.arrStockListStr);
    });

    this.stockService.CompareStocks(this.arrStockListStr).subscribe(res => {
      this.stockReport = res;
      console.log(this.stockReport);
    });
  }

  onViewDetailedReport() {
    this.isShowDetailedReport = !this.isShowDetailedReport;
    if (this.isShowDetailedReport == false)
      this.showHideDetailedReportCaption = 'Show Detailed Report';
    else {
      this.showHideDetailedReportCaption = 'Hide Detailed Report';
      this.stockService.getStockComparisonDetailedData().subscribe(res => {
        if (res.length == 0) {
          this.isNoDataFound = true;
        }
        else {
          console.log(res);
          this.isNoDataFound = false;
          this.stockPL = res.filter(r => r.statement == 'PL');
          this.stockBalanceSheet = res.filter(r => r.statement == 'BALANCESHEET');
          this.stockCashFlow = res.filter(r => r.statement == 'CASHFLOW');
          this.stockRatios = res.filter(r => (r.statement == 'LeverageRatio' || r.statement == 'OperatingRatio ' || r.statement == 'ProfitabilityRatio'));
        }
      });
    }

  }

}
