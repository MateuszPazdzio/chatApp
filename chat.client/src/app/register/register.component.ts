import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { RegisterResponse } from '../models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{

  form!: FormGroup
  registerResponse: RegisterResponse = {
    isRegistered: false,
    responseMessage:""
  }
  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {
  }


  verifyPassword(afc: AbstractControl): {[key:string]:any}  | null
  {
    const password = afc.get("password")?.value;
    const passwordVerification = afc.get("passwordVerification")?.value;
    return password === passwordVerification ? null : {"passwordMismatch":true,"message":"Passwords are not equal"}
  }
  ngOnInit(): void {
    this.form=this.fb.group({
      "username":['',Validators.required],
      "email": ['', [Validators.email, Validators.required]],
      "password": ['',Validators.required],
      "passwordVerification": ['',Validators.required]
    }, { validator: [this.verifyPassword] })
  }

  ngSubmit() {
    if (this.form.valid) {
      this.userService.registerUser(
        {
          email: this.form.get("email")?.value,
          login: this.form.get("login")?.value,
          password: this.form.get("password")?.value,
          passwordVer: this.form.get("passwordVerification")?.value,
          userName: this.form.get("username")?.value
        }
      ).subscribe(data => {
        this.registerResponse = data;
        if (this.registerResponse.isRegistered) {
          this.router.navigate(['/auth/login'])
        }
      })
    }
  }

  //async verifyUserExists(afc: AbstractControl): Promise<{ [key: string]: any; } | null>{
  //  this.userService.checkIfUserWithSpecificEmailOrUserNameExists(
  //    {
  //      searchBy: await this.getControlName(afc),
  //      searchPhrase: afc.value
  //    }
  //  ).subscribe(status => {
  //    this.status = status
  //  });

  //  return (this.status.code == 400) ? { "alreadyExists": true, "message": this.status.message } : null;
  //}
  //async getControlName(control: AbstractControl): Promise<string | null> {
  //  // Ensure the control has a parent and that the parent's controls object is defined
  //  const parent = control.parent as FormGroup;

  //  if (parent && parent.controls) {
  //    // Find the control's name by comparing with its parent form group's controls
  //    return Object.keys(parent.controls).find(name => control === parent.controls[name]) || null;
  //  }

  //  return null;
  //}
}
