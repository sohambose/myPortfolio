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

  uploadTypes: any[] = [
    { "value": "1", "text": "Yearly Data" },
    { "value": '2', "text": "Quarterly Data" }
  ];
  defaultRadio: any = "1";

  lstStocks: any[] = [];
  selectedStockID: any;


  @ViewChild('uploadForm', { static: true }) uploadForm: NgForm;
  @ViewChild('fileFundamental', { static: true }) fileFundamental: ElementRef;

  constructor(private uploadService: UploadService, private stockService: StockService) { }

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.lstStocks = res;
      this.selectedStockID = -1;
    })
  }

  onStockSelectionChange() {
    this.fileFundamental.nativeElement.value = "";
    this.lblFileUpload = "Select File";
  }


  handleFileInput(arrFiles: FileList) {
    console.log(arrFiles[0]);
    this.fileToUpload = arrFiles.item(0);
    this.lblFileUpload = this.fileToUpload.name;
    this.IsFileSelected = true;
  }

  /*  onUploadClick() {
     if (this.selectedStockID < 0) {
       alert('Select a stock from List');
     }
     else {
       this.uploadService.uploadFile(this.fileToUpload, this.selectedStockID).subscribe(res => {
         console.log(res);
         alert("File Uploaded and Processed succesfully!");
         this.fileFundamental.nativeElement.value = "";
         this.lblFileUpload = "Select File";
         this.selectedStockID = -1;
       })
     } 
   }*/

  onSubmit(uploadform: NgForm) {
    if (this.selectedStockID < 0) {
      alert('Select a stock from List');
    }
    else {
      this.uploadService.uploadFile(this.selectedStockID, uploadform.value.uploadtype, this.fileToUpload,).subscribe(res => {
        console.log(res);
        alert("File Uploaded and Processed succesfully!");
        this.fileFundamental.nativeElement.value = "";
        this.lblFileUpload = "Select File";
        this.selectedStockID = -1;
      })
    }
  }

}
