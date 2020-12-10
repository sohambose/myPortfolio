import { Component, OnInit } from '@angular/core';
import { UploadService } from '../_Services/upload.service';

@Component({
  selector: 'app-excel-upload',
  templateUrl: './excel-upload.component.html',
  styleUrls: ['./excel-upload.component.css']
})
export class ExcelUploadComponent implements OnInit {
  fileToUpload: File = null;
  lblFileUpload: string = 'Select File';
  IsFileSelected: boolean = false;

  constructor(private uploadService: UploadService) { }

  ngOnInit(): void {
  }


  handleFileInput(arrFiles: FileList) {
    console.log(arrFiles[0]);
    this.fileToUpload = arrFiles.item(0);
    this.lblFileUpload = this.fileToUpload.name;
    this.IsFileSelected = true;
  }

  onUploadClick() {
    this.uploadService.uploadFile(this.fileToUpload).subscribe(res => {
      console.log(res);
    })
  }

}
