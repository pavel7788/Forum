export interface AddPost {
  title: string;
  summary: string;
  content: string;
  publishDate: Date;
  userId: string;
  userName: string;
  comments: [];
}
