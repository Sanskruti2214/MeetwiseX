import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { JwtHelperService } from '@auth0/angular-jwt';
@Component({
  selector: 'app-login',
  imports: [FormsModule , CommonModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  userEmail:string=''
  userPassword:string=''
  companyId:string=''
  constructor(private userService:UserService,private router:Router){ }
  login(data:any){
    console.log('Form Data:',data);
    this.userService.login(data).subscribe({
      next: (res) => {
        console.log('login successful:', res);
        const token=res.token;
        console.log('check token', token);
        const helper = new JwtHelperService();
        const decodedToken: any = helper.decodeToken(token);
        if(token){
          sessionStorage.setItem('token',token);
          console.log("Email= ",decodedToken.Email);
          sessionStorage.setItem('userId',decodedToken.Email);
          console.log("Role= ",decodedToken.role);
          sessionStorage.setItem('userRole',decodedToken.Role);
          console.log("companyId= ",decodedToken.CompanyId);
          sessionStorage.setItem('companyId',decodedToken.CompanyId);
          alert('Login Successful!');
          this.router.navigate(['/mybookings']);
        }
        else {
          alert('Login failed: Token not found.');
        }
      },
      error: (err) => {
        console.error('login error:', err);
        alert('login Failed!');
      }
    });

  }
}
