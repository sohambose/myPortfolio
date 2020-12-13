import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  baseURL: string = 'https://localhost:5001';

  constructor(private http: HttpClient) { }

  uploadFile(file: File, stockID: number) {
    const formData: FormData = new FormData();
    formData.append('fileuploaded', file, file.name);
    formData.append('stockID', stockID.toString());
    console.log('In Service: ' + file + ' For Stock ID: ' + stockID.toString());
    return this.http.post(this.baseURL + '/api/FileUploads', formData);
  }
}
