import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BusinessCardService {

  private apiUrl = 'https://localhost:7155/api/BusinessCard';  // Correct API URL

  constructor(private http: HttpClient) { }

  // Fetch all business cards
  getBusinessCards(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
  }

  // Create a new business card
  createBusinessCard(businessCard: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, businessCard);
  }

  // Delete a business card by ID
  deleteBusinessCard(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }


  exportBusinessCards(fileType: string): Observable<Blob> {
    const exportUrl = `${this.apiUrl}/export?fileType=${fileType}`;
    return this.http.get(exportUrl, { responseType: 'blob' });
  }

  // Import business cards from file (XML/CSV)
  importBusinessCards(formData: FormData): Observable<any[]> {
    return this.http.post<any[]>(`${this.apiUrl}/import`, formData);
  }
}