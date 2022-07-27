import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../../models/user';
import { AuthService, ERROR_INFO, SUCCESS_INFO, USER_ID } from '../../../services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {

  @ViewChild('newPassword') newPassword: ElementRef;
  @ViewChild('confirmPassword') confirmPassword: ElementRef;

  user: User = {
    id: '',
    userName: '',
    isBanned: false,
    userRoles: '',
    posts: [],
    comments: []
  }

  constructor(
    private as: AuthService,
    private router: Router
  ) { }

  changePassword(newPassword: string, confirmPassword: string): void {
    this.as.changePassword(newPassword, confirmPassword)
      .subscribe((response) => {
        localStorage.setItem(SUCCESS_INFO, "Password changed successfully");
        this.router.navigate(['success']);
      }, error => {
        localStorage.setItem(ERROR_INFO, "Password change failed");
        this.router.navigate(['error']);
      });

  }

}

