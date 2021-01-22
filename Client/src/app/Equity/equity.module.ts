import { SharedModule } from './../Shared/Shared.module';
import { ChartsModule } from 'ng2-charts';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { StockLandingComponent } from './stock-landing/stock-landing.component';
import { StockEntryComponent } from './stock-entry/stock-entry.component';
import { StockFundamentalReportComponent } from './stock-fundamental-report/stock-fundamental-report.component';
import { ExcelUploadComponent } from './excel-upload/excel-upload.component';
import { StockComparisonReportComponent } from './stock-comparison-report/stock-comparison-report.component';
import { StockPortfolioListComponent } from './stock-portfolio-list/stock-portfolio-list.component';

const equityRoutes: Routes = [
    {
        path: 'portfolio', component: StockLandingComponent, children: [
            { path: 'edit', component: StockEntryComponent },
            { path: 'edit/:stockID', component: StockEntryComponent }
        ]
    },
    { path: 'stock-fundamentals/:stockID', component: StockFundamentalReportComponent },
    { path: 'excel-upload', component: ExcelUploadComponent },
    { path: 'compare-stocks/:stockIDList', component: StockComparisonReportComponent }
];

@NgModule({
    declarations: [
        StockEntryComponent,
        StockPortfolioListComponent,
        StockLandingComponent,
        ExcelUploadComponent,
        StockFundamentalReportComponent,
        StockComparisonReportComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ChartsModule,
        SharedModule,
        RouterModule.forChild(equityRoutes)
    ]
})
export class EquityModule {
}