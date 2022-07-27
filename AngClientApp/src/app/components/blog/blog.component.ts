import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Post } from '../../models/post';
import { PostComment } from '../../models/postcomment';
import { AuthService, USER_ID } from '../../services/auth.service';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})

export class BlogComponent implements OnInit {

  constructor(
    private bs: BlogService,
    private as: AuthService
  ) { }

  posts: Post[] = [];
  postComments: PostComment[] = [];

  ngOnInit(): void {    
    this.bs.getPosts()
      .subscribe((response: Post[]) => {
        this.posts = response;
      }, error => console.error(error));
  }

  public get isLoggedIn(): boolean {
    return this.as.isAuthentificated();
  }

  public get isAdmin(): boolean {
    return this.as.isAdmin();
  }

  public get isBanned(): boolean {
    return this.as.isBanned();
  }

  public isPostOwner(post: Post): boolean {
    return (post?.userName == this.as.getUserName());
  }
  
}
