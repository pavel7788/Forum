import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Role } from '../../models/role';
import { ERROR_INFO } from '../../services/auth.service';
import { RolesService } from '../../services/roles.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {

  @ViewChild('role_input') role_input: ElementRef;

  roles: Role[] = [];
  roleModel: Role = {
    id: '',
    name: ''
  }

  constructor(
    private rs: RolesService,
    private router: Router
  ) { }

  getRoles() {
    this.rs.getRoles()
      .subscribe((response: Role[]) => {
        this.roles = response;
      }, error => console.error(error));
  }

  ngOnInit(): void {
    this.getRoles();
  }

  onSubmit(): void {

    if (this.roleModel.id === '') {
      this.rs.addRole(this.roleModel)
        .subscribe((response) => {
          this.getRoles();
          this.role_input.nativeElement.value = '';
          this.roleModel = {
            id: '',
            name: ''
          }
        }, error => console.error(error)
        )
    } else {      
      this.rs.updateRole(this.roleModel)
        .subscribe((response) => {
          this.getRoles();
          this.role_input.nativeElement.value = '';          
        }, error => console.error(error)
        )      
    }
  }

  deleteRole(id: string): void {
    this.rs.deleteRole(id)
      .subscribe((response) => {
        this.getRoles();
        this.role_input.nativeElement.value = '';  
      }, error => {
        localStorage.setItem(ERROR_INFO, "Operation forbidden");
        this.router.navigate(['error']);
        //console.error(error)
      }        
      )
  }

  populateForm(role: Role) {
    this.roleModel = role;
  }

  updateRole(role: Role) {
    this.rs.updateRole(role)
      .subscribe((response) => {
      this.getRoles();
    }, error => console.error(error)
    )
  }

}
