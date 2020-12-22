import { ChartService } from './../_Services/chart.service';
import { Observable } from 'rxjs';
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

  isNoYearlydataFound: boolean;
  isNoQuarterlydataFound: boolean;

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

  TestLabel: string;
  TestgraphColor;
  TestyAxisData: any[];
  TestxAxisData: any[];

  res: Observable<any>[];


  constructor(private activatedRoute: ActivatedRoute, private stockService: StockService,
    private router: Router, private chartService: ChartService) { }

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
        this.isNoYearlydataFound = true;
      }
      else {
        this.isNoYearlydataFound = false;
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
        this.isNoQuarterlydataFound = true;
      }
      else {
        this.isNoQuarterlydataFound = false;
        this.stockQtrlyData = res;
        this.loadGraphData(res, 2);
      }
    });
  }

  loadGraphData(res: any, reportType: number) {
    if (reportType == 1) {
      const arrRevenueData = res.filter(r => r.head == 'RevenueTotalCrores')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisFundamental, arrRevenueData.reverse(), 'Total Revenue', 'green', this.revenueChartCanvas.nativeElement);

      const arrNetProfitData = res.filter(r => r.head == 'NETProfit')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisFundamental, arrNetProfitData.reverse(), 'Net Profit', 'blue', this.netprofitChartCanvas.nativeElement);

      const arrdebtData = res.filter(r => r.head == 'Borrowings')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisFundamental, arrdebtData.reverse(), 'Debt', 'red', this.debtChartCanvas.nativeElement);

      const arrROEData = res.filter(r => r.head == 'ROE')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisFundamental, arrROEData.reverse(), 'Return on Equity', '#33D5FF', this.roeChartCanvas.nativeElement);

      const arrROCEData = res.filter(r => r.head == 'ROCE')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisFundamental, arrROCEData.reverse(), 'Return on Capital Employed', 'blue', this.roceChartCanvas.nativeElement);

    }
    else if (reportType == 2) {
      const arrSalesData = res.filter(r => r.narration == 'Sales')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisQuarterly, arrSalesData.reverse(), 'Quarterly Sales', 'green', this.QSalesChartCanvas.nativeElement);

      const arrNetProfitdata = res.filter(r => r.narration == 'NetProfit')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisQuarterly, arrNetProfitdata.reverse(), 'Quarterly Net Profit', 'green', this.QNetProfitChartCanvas.nativeElement);

      const arrEBITdata = res.filter(r => r.narration == 'EBIT')[0].graphData;
      this.chartVar = this.chartService.drawLineChart(this.xAxisQuarterly, arrEBITdata.reverse(), 'Quarterly EBIT', 'green', this.QEBITChartCanvas.nativeElement);
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
