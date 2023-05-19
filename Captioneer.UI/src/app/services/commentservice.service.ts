import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CommentViewModel } from '../models/comment-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private httpClient : HttpClient) { }

  getSubtitleMovieComments(subtitleID : number, page : number = 1, pageSize : number = 6) : Promise<CommentViewModel[] | null>  {
    return new Promise((resolved : any) => {
      this.httpClient.get(environment.apiURL + `/Comments/Movies/${subtitleID}?page=${page}&pageSize=${pageSize}`).subscribe({
        next: (response) => {
          resolved(response);
        },
        error: (err) => {
          console.error(err.error);
          resolved(null);
        }
      })
    });
  }

  getSubtitleTvShowComments(subtitleID : number, page : number = 1, pageSize : number = 6) : Promise<CommentViewModel[] | null>  {
    return new Promise((resolved : any) => {
      this.httpClient.get(environment.apiURL + `/Comments/Shows/${subtitleID}?page=${page}&pageSize=${pageSize}`).subscribe({
        next: (response) => {
          resolved(response);
        },
        error: (err) => {
          console.error(err.error);
          resolved(null);
        }
      })
    });
  }

  postComment(model : CommentViewModel) : Promise<void> {
    return new Promise(() => {
      this.httpClient.post(environment.apiURL + "/Comments", model).subscribe({
        complete: () => {
          setTimeout(() => {
            window.location.reload();
          }, 1500);
        }
    })
    });
  }
}
