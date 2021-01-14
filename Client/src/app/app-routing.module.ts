import { StockFundamentalReportComponent } from './Equity/stock-fundamental-report/stock-fundamental-report.component';
import { ExcelUploadComponent } from './Equity/excel-upload/excel-upload.component';
import { StockLandingComponent } from './Equity/stock-landing/stock-landing.component';
import { HomeComponent } from './HomeDashboard/home/home.component';
import { StockEntryComponent } from './Equity/stock-entry/stock-entry.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StockComparisonReportComponent } from './Equity/stock-comparison-report/stock-comparison-report.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  {
    path: 'portfolio', component: StockLandingComponent, children: [
      { path: 'edit', component: StockEntryComponent },
      { path: 'edit/:stockID', component: StockEntryComponent },
      { path: 'stock-fundamentals/:stockID', component: StockFundamentalReportComponent }
    ]
  },
  { path: 'excel-upload', component: ExcelUploadComponent },
  { path: 'compare-stocks/:stockIDList', component: StockComparisonReportComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
