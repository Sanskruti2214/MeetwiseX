import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomsService } from '../../services/rooms.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-manage-rooms',
  imports: [CommonModule,FormsModule],
  templateUrl: './manage-rooms.component.html',
  styleUrl: './manage-rooms.component.css'
})
export class ManageRoomsComponent {
  roomType!:string
  roomId!:number
  types:string[]=['Huddle Room','Meeting Room','Board Room','Training Room','Interview Room','Conference Room']
  buildingId!:number
  buildingName!:string
  floorId!:number
  constructor(private roomService:RoomsService,private router:Router ){
  }
  addRoomDetails(data:any){
    this.roomService.addRoomDetails(data).subscribe({
      next: (res) => {
        console.log('Room Added successfully:', res);
        alert('Room Added successfully!');
        this.router.navigate(['/mybookings']);
      },
      error: (err) => {
        console.error('Room Added error:', err);
        alert('Room Added Failed');
      }
    });
    
  }
}
