import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { EquityModule } from './Equity/equity.module';
import { SharedModule } from './Shared/Shared.module';
import { HomeDashboardModule } from './HomeDashboard/HomeDashboard.module';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    EquityModule,
    SharedModule,
    HomeDashboardModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
