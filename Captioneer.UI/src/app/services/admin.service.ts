import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private url: string = environment.apiURL;
  constructor(private http: HttpClient) { }

  deleteComment(commentID: number): Observable<HttpResponse<string>> {
    return this.http.delete(`${this.url}/Comments/${commentID}`, { observe: 'response', responseType: 'text' });
  }

  deleteMovieSubtitle(movieSubtitleID: number): Observable<HttpResponse<string>> {
    return this.http.delete(`${this.url}/SubtitleMovie/${movieSubtitleID}`, { observe: 'response', responseType: 'text' });
  }
}
