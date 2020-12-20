import { StockService } from './../_Services/Stock.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-stock-fundamental-report',
  templateUrl: './stock-fundamental-report.component.html',
  styleUrls: ['./stock-fundamental-report.component.css']
})
export class StockFundamentalReportComponent implements OnInit {

  selectedStockID: number;
  stock: any;
  stockSymbol: string;
  stockCompanyName: string;
  stockIndustry: string;

  stockPL: any[];
  stockBalanceSheet: any[];
  stockCashFlow: any[];
  stockRatios: any[];

  stockQtrlyData: any[];

  isNodataFound: boolean;

  chartVar: any;
  @ViewChild('revenueChartCanvas') private revenueChartCanvas;

  netProfitChart: any;
  @ViewChild('netprofitChartCanvas') private netprofitChartCanvas;

  debtChart: any;
  @ViewChild('debtChartCanvas') private debtChartCanvas;

  roeChart: any;
  @ViewChild('roeChartCanvas') private roeChartCanvas;

  roceChart: any;
  @ViewChild('roceChartCanvas') private roceChartCanvas;

  QSalesChart: any;
  @ViewChild('QSalesChartCanvas') private QSalesChartCanvas;

  QNetProfitChart: any;
  @ViewChild('QNetProfitChartCanvas') private QNetProfitChartCanvas;

  QEBITChart: any;
  @ViewChild('QEBITChartCanvas') private QEBITChartCanvas;


  xAxisFundamental = ['y9', 'y8', 'y7', 'y6', 'y5', 'y4', 'y3', 'y2', 'y1', 'y0'];
  xAxisQuarterly = ['Q9', 'Q8', 'Q7', 'Q6', 'Q5', 'Q4', 'Q3', 'Q2', 'Q1', 'Q0'];

  //------------------------------------------------- 


  constructor(private activatedRoute: ActivatedRoute, private stockService: StockService,
    private router: Router) { }



  ngOnInit(): void {
    this.activatedRoute.params.subscribe(r => {
      this.selectedStockID = r['stockID'];
      this.loadStockDetails();
      this.loadStockFundamentals();
      this.loadStockQuarterlyData();
    })
  }

  closeComponent() {
    this.router.navigate(['/portfolio']);
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
      if (res.length == 0) {
        this.isNodataFound = true;
      }
      else {
        this.isNodataFound = false;
        this.stockPL = res.filter(r => r.statement == 'PL');
        this.stockBalanceSheet = res.filter(r => r.statement == 'BALANCESHEET');
        this.stockCashFlow = res.filter(r => r.statement == 'CASHFLOW');
        this.stockRatios = res.filter(r => (r.statement == 'LeverageRatio' || r.statement == 'OperatingRatio ' || r.statement == 'ProfitabilityRatio'));
        this.loadGraphData(res, 1);
      }
    });
  }

  loadStockQuarterlyData() {
    this.stockService.getStockQuarterlyData(this.selectedStockID).subscribe(res => {
      if (res.length == 0) {
        this.isNodataFound = true;
      }
      else {
        this.isNodataFound = false;
        this.stockQtrlyData = res;
        this.loadGraphData(res, 2);
      }
    });
  }

  loadGraphData(res: any, reportType: number) {
    if (reportType == 1) {
      const arrRevenueData = res.filter(r => r.head == 'RevenueTotalCrores')[0].graphData;
      this.drawGraph('Total Revenue', 'green', arrRevenueData.reverse(), this.xAxisFundamental, this.revenueChartCanvas.nativeElement);

      const arrNetProfitData = res.filter(r => r.head == 'NETProfit')[0].graphData;
      this.drawGraph('Net Profit', 'blue', arrNetProfitData.reverse(), this.xAxisFundamental, this.netprofitChartCanvas.nativeElement);

      const arrdebtData = res.filter(r => r.head == 'Borrowings')[0].graphData;
      this.drawGraph('Debt', 'red', arrdebtData.reverse(), this.xAxisFundamental, this.debtChartCanvas.nativeElement);

      const arrROEData = res.filter(r => r.head == 'ROE')[0].graphData;
      this.drawGraph('Return on Equity', '#33D5FF', arrROEData.reverse(), this.xAxisFundamental, this.roeChartCanvas.nativeElement);

      const arrROCEData = res.filter(r => r.head == 'ROCE')[0].graphData;
      this.drawGraph('Return on Capital Employed', 'blue', arrROCEData.reverse(), this.xAxisFundamental, this.roceChartCanvas.nativeElement);

    }
    else if (reportType == 2) {
      console.log('Qtrly Data Graph----');
      console.log(res);
      const arrSalesData = res.filter(r => r.narration == 'Sales')[0].graphData;
      this.drawGraph('Quarterly Sales', 'green', arrSalesData.reverse(), this.xAxisQuarterly, this.QSalesChartCanvas.nativeElement);

      const arrNetProfitdata = res.filter(r => r.narration == 'NetProfit')[0].graphData;
      this.drawGraph('Quarterly Net Profit', 'green', arrNetProfitdata.reverse(), this.xAxisQuarterly, this.QNetProfitChartCanvas.nativeElement);

      const arrEBITdata = res.filter(r => r.narration == 'EBIT')[0].graphData;
      this.drawGraph('Quarterly EBIT', 'green', arrEBITdata.reverse(), this.xAxisQuarterly, this.QEBITChartCanvas.nativeElement);
    }
  }

  drawGraph(dataTypeLabel: string, graphColor: string, arrData: any[], xAxisData: any[], canvasRef: any) {
    this.chartVar = new Chart(canvasRef, {
      type: 'line',
      data: {
        labels: xAxisData, // your labels array
        datasets: [
          {
            label: dataTypeLabel,
            data: arrData, // your data array
            borderColor: graphColor,
            fill: false,
            lineTension: 0,
            spanGaps: true
          }
        ]
      },
      options: {
        legend: {
          display: true
        },
        scales: {
          xAxes: [{
            display: true
          }],
          yAxes: [{
            display: false
          }],
        }
      }
    });
  }

}
