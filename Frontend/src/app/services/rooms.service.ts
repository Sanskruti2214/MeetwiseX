import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class RoomsService {
  private apiUrl='http://localhost:5165/api/Dashboard';
  constructor(private http:HttpClient) { }
  
  addRoomDetails(userData:any){
    return this.http.post(`${this.apiUrl}/addRoom`,userData);
  }

  reservingRoom(userData:any){
    return this.http.post(`${this.apiUrl}/reserve`,userData);
  }

  showMyBookings(){
    return this.http.get(`${this.apiUrl}/mybookings`);
  }
  
  meetScheduling(userData:any){
    return this.http.post(`${this.apiUrl}/scheduleMeeting`,userData);
  }

  // showAllBookings():Observable<bookings[]>{
  //   return this.http.get<bookings[]>(`${this.apiUrl}\bookings`);
  // }
  
}
