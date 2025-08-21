import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RoomsService } from '../../services/rooms.service';
import { bookings } from '../../models/bookings';
import { Router } from '@angular/router';
@Component({
  selector: 'app-reserve-room',
  imports: [FormsModule,CommonModule],
  templateUrl: './reserve-room.component.html',
  styleUrl: './reserve-room.component.css'
})
export class ReserveRoomComponent {
  minDateTime:string=''
  roomsType!:string
  types:string[]=['Huddle Room','Meeting Room','Board Room','Training Room','Interview Room','Conference Room']
  buildingId!:number
  buildingName!:string
  floorId!:number
  startTime!:Date
  endTime!:Date
  reserved!:any[]
  isFind:boolean=false;
  constructor(private roomService:RoomsService,private router:Router ){
  }
  ngOnInit(): void {
    const now = new Date();

    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');

    this.minDateTime = `${year}-${month}-${day}T${hours}:${minutes}`;
  };
  reservingRoom(data:any){
    console.log('Sending Reservation Data');
    this.roomService.reservingRoom(data).subscribe({
      next: (res) => {
        console.log('Room Registration successful:', res);
        if(res==null){
          alert('No Room Available!');
        }else{
          alert('Room Registration successful!');
        }
        this.router.navigate(['/mybookings']);
      },
      error: (err) => {
        console.error('Room Registration  error:', err);
        alert('Room Registration Failed');
      }
    });
  }

}
