import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { isPlatformBrowser } from '@angular/common';
import { UserService } from '../../services/user.service';
@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  isAdmin: boolean = false;
  isLoggedIn = false;
  initialized = false;
  constructor(private userService: UserService, private router: Router, @Inject(PLATFORM_ID) private platformId: Object) {
    //  const token = sessionStorage.getItem('token');
    // const role = sessionStorage.getItem('Role');

    // this.userService.loggedIn.next(!!token);
    // this.userService.isAdmin.next(role?.trim().toLowerCase() === 'admin');
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const token = sessionStorage.getItem('token');
      const role = sessionStorage.getItem('Role');

      this.userService.loggedIn.next(!!token);

      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          console.log('Decoded Payload:', payload);
          console.log('Header isAdmin',payload.role);
          this.userService.isAdmin.next(payload.role?.trim().toLowerCase() === 'admin');
        } catch (err) {
          console.error('Invalid token format:', err);
        }
      }
      

      this.userService.loggedIn.subscribe(status => {
        this.isLoggedIn = status;
        console.log("isLoggedIn", status);
        this.initialized = true;
      });

      this.userService.isAdmin.subscribe(adminStatus => {
        this.isAdmin = adminStatus;
        console.log("Is Admin:", adminStatus);
      });
    }
  }


  logout() {
    sessionStorage.clear(); // or remove token and role individually
    this.isLoggedIn = false;
    this.router.navigate(['/login']);
  }

}
