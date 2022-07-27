import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt'
import { API_URL } from '../app-injection-tokens';
import { Token } from '../models/token';
import { User } from '../models/user';

export const ACCESS_TOKEN_KEY = 'application_access_token';
export const USER_NAME = 'application_user_name';
export const USER_ID = 'application_user_id';
export const IS_ADMIN = 'application_user_is_admin';
export const IS_BANNED = 'application_user_is_banned';
export const ERROR_INFO = 'error_info';
export const SUCCESS_INFO = 'success_info';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    @Inject(API_URL) private apiUrl: string,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) { }

  register(email: string, password: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}api/auth/register`, {
      email, password
    }).pipe(
      tap(() => this.router.navigate(['auth/login']))
      );
  }

  login(email: string, password: string): Observable<Token> {
    return this.http.post<Token>(`${this.apiUrl}api/auth/login`, {
      email, password
    }).pipe(
      tap(token => {
        localStorage.setItem(ACCESS_TOKEN_KEY, token.access_token);
        localStorage.setItem(USER_NAME, token.email);
        localStorage.setItem(USER_ID, token.id);
        localStorage.setItem(IS_ADMIN, String(token.isAdmin));
        localStorage.setItem(IS_BANNED, String(token.isBanned));
      })
    )
  }

  isAuthentificated(): boolean {
    var token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return (token != null) && !this.jwtHelper.isTokenExpired(token);
  }

  isAdmin(): boolean {
    var isBanned = localStorage.getItem(IS_ADMIN);
    return isBanned == "true";
  }

  logout(): void {
    this.router.navigate(['']);
    localStorage.clear();
  } 

  getUserName(): string | null {
    return localStorage.getItem(USER_NAME);
  }

  getUserId(): string | null {
    return localStorage.getItem(USER_ID);
  }

  isBanned(): boolean {
    var isBanned = localStorage.getItem(IS_BANNED);
    return isBanned == "true";
  }

  changePassword(newPassword: string, confirmPassword: string): Observable<void> {
    var id = this.getUserId();
    return this.http.post<void>(`${this.apiUrl}api/auth/profile/${id}`, {
      newPassword, confirmPassword
    });
  }

}
