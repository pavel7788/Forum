import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router'; 
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { BlogComponent } from './components/blog/blog.component';
import { RolesComponent } from './components/roles/roles.component';
import { LoginComponent } from './components/authorization/login/login.component';
import { RegisterComponent } from './components/authorization/register/register.component';
import { ProfileComponent } from './components/authorization/profile/profile.component';
import { UsersComponent } from './components/users/users.component';
import { AuthGuard } from './guards/auth.guard';
import { ErrorComponent } from './components/error/error.component';
import { SuccessComponent } from './components/success/success.component';
import { PostComponent } from './components/post/post.component';
import { PostAddComponent } from './components/post-add/post-add.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'blog', component: BlogComponent },
  { path: 'blog/add', component: PostAddComponent },
  { path: 'blog/:id', component: PostComponent },
  { path: 'roles', component: RolesComponent, canActivate: [AuthGuard] },
  { path: 'users', component: UsersComponent, canActivate: [AuthGuard] },
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { path: 'auth/profile/:id', component: ProfileComponent },
  { path: 'error', component: ErrorComponent },
  { path: 'success', component: SuccessComponent },


]



@NgModule({
  //declarations: [],
  imports: [
    //CommonModule
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
