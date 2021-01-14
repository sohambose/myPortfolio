import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  baseURL: string = 'https://localhost:5001';

  constructor(private http: HttpClient) { }

  uploadFile(stockID: number, uploadType: any, uploadedfile: File,) {
    const formData: FormData = new FormData();
    formData.append('stockID', stockID.toString());
    formData.append('uploadtype', uploadType.toString());
    formData.append('fileuploaded', uploadedfile, uploadedfile.name);
    
    return this.http.post(this.baseURL + '/api/FileUploads', formData);
  }
}
