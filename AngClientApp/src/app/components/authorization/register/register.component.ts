import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, ERROR_INFO, SUCCESS_INFO } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  @ViewChild('email') email: ElementRef;
  @ViewChild('password') password: ElementRef;

  constructor(
    private as: AuthService,
    private router: Router
  ) { }

  register(email: string, password: string) {
    this.as.register(email, password)
      .subscribe(res => {
        localStorage.setItem(SUCCESS_INFO, "Registration successfull");
        this.router.navigate(['success']);
      }, error => {
        localStorage.setItem(ERROR_INFO, "Registration failed");
        this.router.navigate(['error']);
        //this.email.nativeElement.value = '';
        //this.password.nativeElement.value = '';
      })
  }

}
