import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(private as: AuthService) {

  }

  public get isLoggedIn(): boolean {
    return this.as.isAuthentificated();
  }

  public get isAdmin(): boolean {
    return this.as.isAdmin();
  }

  public logout() {
    this.as.logout();
  }

  getUserName(): string | null {
    return this.as.getUserName();
  }

  getUserId(): string | null {
    return this.as.getUserId();
  }

}
