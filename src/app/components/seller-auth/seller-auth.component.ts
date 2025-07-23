import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-seller-auth',
  imports: [FormsModule,CommonModule],
  templateUrl: './seller-auth.component.html',
  styleUrl: './seller-auth.component.css'
})
export class SellerAuthComponent {
  userEmail: string = ''
  userName: string =''
  companyName:string=''
  companyUniqueId:string=''
  userPassword: string =''

  constructor(private userService : UserService ,private router:Router) {}
  signUp(data: any) {
    console.log('Form data:', data);
    this.userService.signUp(data).subscribe({
      next: (res) => {
        console.log('Signup successful:', res);
        alert('Signup Successful!');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Signup error:', err);
        alert('Signup Failed!');
      }
    });
  }
}
