import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home/home.component';
import { BlogComponent } from './components/blog/blog.component';
import { AppRoutingModule } from './app-routing.module';
import { API_URL } from './app-injection-tokens';
import { environment } from '../environments/environment';
import { RolesComponent } from './components/roles/roles.component';

import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { LoginComponent } from './components/authorization/login/login.component';
import { RegisterComponent } from './components/authorization/register/register.component';
import { ProfileComponent } from './components/authorization/profile/profile.component';
import { UsersComponent } from './components/users/users.component';
import { FormsModule } from '@angular/forms';
import { ErrorComponent } from './components/error/error.component';
import { SuccessComponent } from './components/success/success.component';
import { PostComponent } from './components/post/post.component';
import { PostAddComponent } from './components/post-add/post-add.component';

export function tokenGetter() {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BlogComponent,
    RolesComponent,
    NavMenuComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
    UsersComponent,
    ErrorComponent,
    SuccessComponent,
    PostComponent,
    PostAddComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, BrowserAnimationsModule, AppRoutingModule,
    MatCardModule, MatInputModule, MatButtonModule, MatTableModule, MatFormFieldModule,
    MatToolbarModule, FormsModule,

    JwtModule.forRoot({
        config: {
          tokenGetter,          
          allowedDomains: ['localhost:5000']       
        }
      })
  ],
  providers: [
    {
      provide: API_URL,
      useValue: environment.baseUrl
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
