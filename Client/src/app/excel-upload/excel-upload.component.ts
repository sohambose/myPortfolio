import { StockService } from './../_Services/Stock.service';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { UploadService } from '../_Services/upload.service';
import { Form, NgForm } from '@angular/forms';


@Component({
  selector: 'app-excel-upload',
  templateUrl: './excel-upload.component.html',
  styleUrls: ['./excel-upload.component.css']
})
export class ExcelUploadComponent implements OnInit {
  fileToUpload: File = null;
  lblFileUpload: string = 'Select File';
  IsFileSelected: boolean = false;

  lstStocks: any[] = [];
  selectedStock: any;

  @ViewChild('fileFundamental', { static: true }) fileFundamental: ElementRef;

  constructor(private uploadService: UploadService, private stockService: StockService) { }

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.lstStocks = res;
      this.selectedStock = -1;
    })
  }

  handleSelection() {
    this.fileFundamental.nativeElement.value = "";
    this.lblFileUpload = "Select File";
  }


  handleFileInput(arrFiles: FileList) {
    console.log(arrFiles[0]);
    this.fileToUpload = arrFiles.item(0);
    this.lblFileUpload = this.fileToUpload.name;
    this.IsFileSelected = true;
  }

  onUploadClick() {
    if (this.selectedStock < 0) {
      alert('Select a stock from List');
    }
    else {
      this.uploadService.uploadFile(this.fileToUpload, this.selectedStock).subscribe(res => {
        console.log(res);
      })
    }
  }

}
