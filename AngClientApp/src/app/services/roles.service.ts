import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';
import { Role } from '../models/role';

@Injectable({
  providedIn: 'root'
})
export class RolesService {

  private url = environment.baseUrl + 'api/roles';

  constructor(private http: HttpClient) { }

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.url);
  }

  addRole(role: Role): Observable<Role> {
    return this.http.post<Role>(this.url, role);
  }

  deleteRole(id: string): Observable<Role> {
    return this.http.delete<Role>(this.url + '/' + id);
  }

  updateRole(role: Role): Observable<Role> {
    return this.http.put<Role>(this.url + '/' + role.id, role);
  }
    
}
