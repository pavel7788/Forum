import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AddPost } from '../models/add-post';
import { AddPostComment } from '../models/add-postcomment';
import { Post } from '../models/post';
import { PostComment } from '../models/postcomment';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  private posts_url = environment.baseUrl + 'api/posts';
  private comments_url = environment.baseUrl + 'api/comments';

  constructor(private http: HttpClient) { }  

  getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(this.posts_url);
  }

  getPostById(id: string): Observable<Post> {
    return this.http.get<Post>(this.posts_url + '/' +id);
  }  

  updatePost(id: string | undefined, post: Post | undefined): Observable<Post> {
    return this.http.put<Post>(this.posts_url + '/' + id, post);
  }

  addPost(post: AddPost): Observable<AddPost> {
    return this.http.post<AddPost>(this.posts_url, post);
  }

  deletePost(id: string | undefined): Observable<Post> {
    return this.http.delete<Post>(this.posts_url + '/' + id);
  }

  getCommentsByPostId(id: string ): Observable<PostComment[]> {
    return this.http.get<PostComment[]>(this.posts_url + '/' + id +'/comments');
  }

  addComment(postComment: AddPostComment): Observable<AddPostComment> {
    return this.http.post<AddPostComment>(this.comments_url, postComment);
  }

  deleteComment(id: string | undefined): Observable<PostComment> {
    return this.http.delete<PostComment>(this.comments_url + '/' + id);
  }
  updateComment(id: string | undefined, postComment: PostComment | undefined): Observable<PostComment> {
    return this.http.put<PostComment>(this.comments_url + '/' + id, postComment);
  }
}
