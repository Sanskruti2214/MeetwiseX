import { Routes } from '@angular/router';
import {HomeComponent} from './components/home/home.component'
import {SellerAuthComponent} from './components/seller-auth/seller-auth.component'
import { LoginComponent } from './components/login/login.component';
import { MyBookingsComponent } from './components/my-bookings/my-bookings.component';
import { ReserveRoomComponent } from './components/reserve-room/reserve-room.component';
import { ManageRoomsComponent } from './components/manage-rooms/manage-rooms.component';
import { SchedulemeetComponent } from './components/schedulemeet/schedulemeet.component';
export const routes: Routes = [
    {
      component:HomeComponent,
      path:'home'
    },
    {
      component:SellerAuthComponent,
      path:'sellerauth'
    },
    {
      component:LoginComponent,
      path:'login'
    },
    {
      component:MyBookingsComponent,
      path:'mybookings'
    },
    {
      component:ReserveRoomComponent,
      path:'reserveroom'
    },
    {
      component:ManageRoomsComponent,
      path:'managerooms'
    },
    {
      component:SchedulemeetComponent,
      path:'schedulemeet'
    }
   
  
];
