import { Injectable } from '@angular/core';
import * as Chart from 'chart.js';

@Injectable({
  providedIn: 'root'
})
export class ChartService {
  chartVar: any;

  constructor() { }

  drawLineChart(xAxisData: any[], yAxisData: any[], dataTypeLabel: string, graphColor: string, canvasRef: any) {
    this.chartVar = new Chart(canvasRef, {
      type: 'line',
      data: {
        labels: xAxisData, // your labels array
        datasets: [
          {
            label: dataTypeLabel,
            data: yAxisData, // your data array
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

    return this.chartVar;
  }

}
