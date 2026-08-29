import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private http = inject(HttpClient);
  // TODO: Replace with the actual API endpoint
  private apiUrl = 'https://localhost:7085/api/auth/login';

  login(credentials: any): Observable<any> {
    return this.http.post(this.apiUrl, credentials);
  }
}
