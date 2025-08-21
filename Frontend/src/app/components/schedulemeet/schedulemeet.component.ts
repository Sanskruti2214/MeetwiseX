import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RoomsService } from '../../services/rooms.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
@Component({
  selector: 'app-schedulemeet',
  imports: [FormsModule, CommonModule],
  templateUrl: './schedulemeet.component.html',
  styleUrl: './schedulemeet.component.css'
})
export class SchedulemeetComponent {
  buildingId!: number
  roomsType!: string
  types: string[] = ['Huddle Room', 'Meeting Room', 'Board Room', 'Training Room', 'Interview Room', 'Conference Room']
  buildingName!: string
  floorId!: number
  date!: Date
  duration!: string
  officeStartTime!: string
  officeEndTime!: string
  constructor(private roomService: RoomsService, private router: Router) {
  }
  convertTimeToMinutes(timeStr: string): number {
    const [hours, minutes] = timeStr.split(':').map(Number);
    return hours * 60 + minutes;
  }
  
  meetScheduling(data: any) {
    const durationInMinutes = this.convertTimeToMinutes(this.duration);
    data.duration = durationInMinutes;
    console.log('Sending Scheduling Data');
    this.roomService.meetScheduling(data).subscribe({
      next: (res) => {
        console.log('Meet Schedule successful:', res);
        if (res == null) {
          alert('No Room Available!');
        } else {
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
