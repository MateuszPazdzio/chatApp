import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CurrentUser, DecodedToken, User } from '../models/User';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  initialize() {
    const token = localStorage.getItem("authToken");
    if (token) {
      const decoded: any = jwtDecode<any>(token);
      console.log(decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'])
      this.currentUserSubject.next({
        user: {
          email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? '',
          userName: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ?? ''
        },
        isAuthenticated: true
      });

    }
  }
  private currentUserSubject = new BehaviorSubject<CurrentUser>({
    user: undefined,
    isAuthenticated: false
  })

  currentUser$ = this.currentUserSubject.asObservable();
  constructor() {
    
  }

  updateCurrentUser(user: CurrentUser) {
    this.currentUserSubject.next(user);
  }

  logout() {
    localStorage.removeItem("authToken");
    this.currentUserSubject.next({
      user: undefined,
      isAuthenticated: false
    });
  }

  isAuthenticated(): boolean{
    return localStorage.getItem("authToken") != null;
  }

  getCurrentUser() {

    return this.currentUserSubject.getValue().user;
  }
}
