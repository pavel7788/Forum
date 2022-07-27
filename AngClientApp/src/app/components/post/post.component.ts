import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AddPostComment } from '../../models/add-postcomment';
import { Post } from '../../models/post';
import { PostComment } from '../../models/postcomment';
import { AuthService, USER_ID } from '../../services/auth.service';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private bs: BlogService,
    private as: AuthService,
    private router: Router
  ) { }
   

  post: Post | undefined;
  postComments: PostComment[] | undefined;

  addPostComment: AddPostComment = {
    content: '',
    publishDate: new Date(),
    postId: '',
    title: '',
    userId: '',
    userName: '',
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      params => {
        const id = params.get('id')

        if (id) {
          this.bs.getPostById(id)
            .subscribe((response) => {
              this.post = response;
            }
          )
          this.bs.getCommentsByPostId(id)
            .subscribe((response) => {
              this.postComments = response;
            }
          )
        }
      })
  }

  onSubmit(): void {
    this.bs.updatePost(this.post?.id, this.post)
      .subscribe((response) => {
        this.router.navigate(['blog']);
      }      
    )
  }

  deletePost(): void {
    this.bs.deletePost(this.post?.id)
      .subscribe((response) => {
        this.router.navigate(['blog']);
      });
  }
  addComment(): void {
    var userId = localStorage.getItem(USER_ID);
    if (userId != null)
      this.addPostComment.userId = userId;
    var postId = this.post?.id;
    if (postId != null)
      this.addPostComment.postId = postId;
    this.bs.addComment(this.addPostComment)
      .subscribe((response) => {
        this.router.navigate(['blog']);
      }
    )
  }

  deleteComment(postComment: PostComment): void {
    this.bs.deleteComment(postComment.id)
      .subscribe((response) => {
        this.router.navigate(['blog']);
      });
  }

  updateComment(postComment: PostComment): void {
    this.bs.updateComment(postComment.id, postComment)
      .subscribe((response) => {
        this.router.navigate(['blog']);
      })
  }

  public get isPostOwner(): boolean {
    return (this.post?.userName == this.as.getUserName());    
  }

  public isPostCommentOwner(postComment: PostComment): boolean {
    return (postComment?.userName == this.as.getUserName());
  }

  public get isAdmin(): boolean {
    return this.as.isAdmin();
  }
  public get isLoggedIn(): boolean {
    return this.as.isAuthentificated();
  }
  public get isBanned(): boolean {
    return this.as.isBanned();
  }
}
