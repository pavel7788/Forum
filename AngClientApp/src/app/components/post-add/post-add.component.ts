import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddPost } from '../../models/add-post';
import { Post } from '../../models/post';
import { AuthService, USER_ID } from '../../services/auth.service';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-post-add',
  templateUrl: './post-add.component.html',
  styleUrls: ['./post-add.component.css']
})
export class PostAddComponent implements OnInit {

  constructor(
    private bs: BlogService,
    private as: AuthService,
    private router: Router,
  ) { }

  post: AddPost = {
    title: '',
    summary: '',
    content: '',
    publishDate: new Date(),
    userId: '',
    userName: '',
    comments: [],
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    var userId = localStorage.getItem(USER_ID);
    if (userId != null)
      this.post.userId = userId;
    this.bs.addPost(this.post)
      .subscribe((response) => {
        this.router.navigate(['blog']);
      }
      )
  }

  public get isBanned(): boolean {
    return this.as.isBanned();
  }

}
