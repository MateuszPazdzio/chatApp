import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { jwtDecode } from 'jwt-decode';
import { CurrentUser, DecodedToken } from '../models/User';
import { AuthService } from '../services/auth.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  constructor(private userService: UserService, private authService:AuthService,private router:Router) {

  }
  logout() {
    this.userService.logoutUser();
    this.authService.logout();
    this.router.navigate(["/auth/login"])
  }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser=user 
    });
  }

  currentUser: CurrentUser = {
    user: undefined,
    isAuthenticated: false
  };
}
