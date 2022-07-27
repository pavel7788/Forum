import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { USER_ID } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private url = environment.baseUrl + 'api/users';

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.url);
  }
  getUser(): Observable<User> {
    var id = localStorage.getItem(USER_ID);
    return this.http.get<User>(this.url + '/' + id);
  }
  isUserAdmin(user: User): boolean {
    return (user.userRoles.includes("Admin"));
  }
  banUser(user: User): Observable<void> {
    return this.http.put<void>(this.url + '/' + user.id + '/ban', {
      
    }).pipe(
      tap(() => this.router.navigate(['']))
    );
  }
  deleteUser(id: string): Observable<User> {
    return this.http.delete<User>(this.url + '/' + id);
  }
}
