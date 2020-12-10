import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  baseURL: string = 'https://localhost:5001';

  constructor(private http: HttpClient) { }

  uploadFile(file: File) {
    const formData: FormData = new FormData();
    formData.append('fileKey', file, file.name);
    console.log('In Service: ' + file);
    return this.http.post(this.baseURL + '/api/FileUploads', formData);
  }
}
