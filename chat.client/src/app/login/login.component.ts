import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { NgForm } from '@angular/forms';
import { DecodedToken, LoginResponse, LoginUserCred } from '../models/User';
import { Route, Router } from '@angular/router';
import { jwtDecode } from "jwt-decode";
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginResponse: LoginResponse = {
    isLoggedIn: false,
    responseMessage: "",
    token:""
  }
  loginUserCred: LoginUserCred = {
    userName: "",
    password:""
  }
  constructor(private userService: UserService,private router:Router, private authService:AuthService) {

  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.userService.loginUser(
        {
          userName: form.value['userName'],
          password:form.value['password']
        }
      ).subscribe(data => {
        this.loginResponse = data
        if (this.loginResponse.isLoggedIn) {
          localStorage.setItem("authToken", this.loginResponse.token)
          const decoded: any = jwtDecode<any>(this.loginResponse.token);
          console.log(decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'])
          this.authService.updateCurrentUser({
            user: {
              email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? '',
              userName: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ?? ''
            },
            isAuthenticated: true
          });
          this.router.navigate(["/chat"])
        }
      })
    }
  }

}
