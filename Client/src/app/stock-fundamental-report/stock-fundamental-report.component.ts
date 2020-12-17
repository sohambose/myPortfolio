import { StockService } from './../_Services/Stock.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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

  isNodataFound: boolean;






  //---------Chart Properties----------------------- 
  /*  lineChartType = 'line';
   xAxis = ['y9', 'y8', 'y7', 'y6', 'y5', 'y4', 'y3', 'y2', 'y1', 'y0'];
   chartLabels = this.xAxis;
 
   arrRevenueData: any[] = [];
   arrDebt: any[] = [];
   revenuechartData: any[];
   debtChartData: any[]; */


  revenueChart: any;
  @ViewChild('revenueChartCanvas') private revenueChartCanvas;

  netProfitChart: any;
  @ViewChild('netprofitChartCanvas') private netprofitChartCanvas;

  debtChart: any;
  @ViewChild('debtChartCanvas') private debtChartCanvas;

  roeChart: any;
  @ViewChild('roeChartCanvas') private roeChartCanvas;

  roceChart: any;
  @ViewChild('roceChartCanvas') private roceChartCanvas;

  xAxis = ['y9', 'y8', 'y7', 'y6', 'y5', 'y4', 'y3', 'y2', 'y1', 'y0'];
  //arrRevenueData: any[] = [];

  //------------------------------------------------- 


  constructor(private activatedRoute: ActivatedRoute, private stockService: StockService,
    private router: Router) { }



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
      if (res.length == 0) {
        this.isNodataFound = true;
      }
      else {
        this.isNodataFound = false;
        this.stockPL = res.filter(r => r.statement == 'PL');
        this.stockBalanceSheet = res.filter(r => r.statement == 'BALANCESHEET');
        this.stockCashFlow = res.filter(r => r.statement == 'CASHFLOW');
        this.stockRatios = res.filter(r => (r.statement == 'LeverageRatio' || r.statement == 'OperatingRatio ' || r.statement == 'ProfitabilityRatio'));
        this.loadGraphData(res);
      }
    });
  }

  loadGraphData(res: any) {
    const arrRevenueData = res.filter(r => r.head == 'RevenueTotalCrores')[0].graphData;
    this.drawGraph('Total Revenue', 'green', arrRevenueData.reverse(), this.revenueChartCanvas.nativeElement);

    const arrNetProfitData = res.filter(r => r.head == 'NETProfit')[0].graphData;
    this.drawGraph('Net Profit', 'blue', arrNetProfitData.reverse(), this.netprofitChartCanvas.nativeElement);

    const arrdebtData = res.filter(r => r.head == 'Borrowings')[0].graphData;
    this.drawGraph('Debt', 'red', arrdebtData.reverse(), this.debtChartCanvas.nativeElement);

    const arrROEData = res.filter(r => r.head == 'ROE')[0].graphData;
    this.drawGraph('Return on Equity', 'black', arrROEData.reverse(), this.roeChartCanvas.nativeElement);

    const arrROCEData = res.filter(r => r.head == 'ROCE')[0].graphData;
    this.drawGraph('Return on Capital Employed', 'black', arrROCEData.reverse(), this.roceChartCanvas.nativeElement);

  }

  closeComponent() {
    this.router.navigate(['/portfolio']);
  }

  drawGraph(dataTypeLabel: string, graphColor: string, arrData: any[], canvasRef: any) {
    this.revenueChart = new Chart(canvasRef, {
      type: 'line',
      data: {
        labels: this.xAxis, // your labels array
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
            display: true
          }],
        }
      }
    });
  }

}
