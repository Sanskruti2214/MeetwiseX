import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { bookings } from '../../models/bookings';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RoomsService } from '../../services/rooms.service';
@Component({
  selector: 'app-my-bookings',
  imports: [CommonModule],
  templateUrl: './my-bookings.component.html',
  styleUrl: './my-bookings.component.css'
})
export class MyBookingsComponent {
  userEmail!: string
  MyBookings!: any[];
  loading: boolean = true;
  constructor(private userService: UserService, private roomService: RoomsService, private router: Router) { }
  ngOnInit() {
    this.loading = true;
    const token = sessionStorage.getItem('token');
    if (token) {
      this.roomService.showMyBookings().subscribe({
        next: (res) => {
          console.log("Mybookings Data !");
          this.MyBookings = res as any[];
          console.log("RoomType =", this.MyBookings[0].roomType);
          console.log("IsExpired =", this.MyBookings[0].IsExpired);
          this.loading = false;
        },
        error: (res) => {
          console.log("Error!");
          this.loading = false;
        }
      });
    }
    else{
      this.router.navigate(['/login']);
    }
  }

}
