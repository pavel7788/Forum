import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user';
import { ERROR_INFO, SUCCESS_INFO, IS_BANNED} from '../../services/auth.service';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  users: User[] = []

  constructor(
    private us: UsersService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.us.getUsers()
      .subscribe((response: User[]) => {
        this.users = response;
      }, error => console.error(error));
  }

  public isUserAdmin(user: User): boolean {
    return this.us.isUserAdmin(user);
  }

  banUser(user: User) {
    this.us.banUser(user)
      .subscribe(res => {
        if (user.isBanned) {
          localStorage.setItem(IS_BANNED, "true");
          localStorage.setItem(ERROR_INFO, "User banned");
          this.router.navigate(['error']);
        } else {
          localStorage.setItem(IS_BANNED, "false");
          localStorage.setItem(SUCCESS_INFO, "User unbanned");
          this.router.navigate(['success']);
        }
      }, error => {
        
      })
  }

  deleteUser(id: string): void {
    this.us.deleteUser(id)
      .subscribe((response) => {
        this.getUsers();
      }, error => {
        localStorage.setItem(ERROR_INFO, "Operation forbidden");
        this.router.navigate(['error']);
      }
      )
  }
  

}
