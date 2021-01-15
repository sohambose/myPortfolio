import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  //baseURL: string = 'https://localhost:5001';
  baseURL: string = environment.baseAPIURL;

  constructor(private http: HttpClient) { }

  uploadFile(stockID: number, uploadType: any, uploadedfile: File,) {
    const formData: FormData = new FormData();
    formData.append('stockID', stockID.toString());
    formData.append('uploadtype', uploadType.toString());
    formData.append('fileuploaded', uploadedfile, uploadedfile.name);

    return this.http.post(this.baseURL + '/api/FileUploads', formData);
  }
}
