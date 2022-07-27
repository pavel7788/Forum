export interface Post {
  id: string;
  title: string;
  summary: string;
  content: string;
  publishDate: Date;
  userId: string;
  userName: string;
  comments: [];
}
