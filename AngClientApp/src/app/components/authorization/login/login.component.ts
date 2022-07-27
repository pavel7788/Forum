import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, ERROR_INFO } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  @ViewChild('email') email: ElementRef;
  @ViewChild('password') password: ElementRef;

  constructor(
    private as: AuthService,
    private router: Router
  ) {}

  login(email: string, password: string) {
    this.as.login(email, password)
      .subscribe(res => {

      }, error => {
        localStorage.setItem(ERROR_INFO, "Login failed");
        this.router.navigate(['error']);
        //this.email.nativeElement.value = '';
        //this.password.nativeElement.value = '';
      })
    
  }

}
