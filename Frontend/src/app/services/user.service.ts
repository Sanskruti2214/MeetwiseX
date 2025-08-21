import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { bookings } from '../models/bookings';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5165/api/User';
  loggedIn = new BehaviorSubject<boolean>(false);
  isAdmin = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this.loggedIn.asObservable();
  isAdmin$ = this.isAdmin.asObservable();
  constructor(private http: HttpClient) { }

  signUp(userData: any) {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  login(Credentials: { email: string, password: string }): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, Credentials).pipe(
      tap(response => {
        sessionStorage.setItem('token', response.token);
        // update observables
        const payload = JSON.parse(atob(response.token.split('.')[1]));
        console.log('Decoded Payload:', payload);

        this.loggedIn.next(true);
        const role = String(payload.role).trim().toLowerCase();
        if(role==='admin'){
          this.isAdmin.next(true);
        }
        
      })
    );
  }

  logout() {
    this.loggedIn.next(false);
  }






}
