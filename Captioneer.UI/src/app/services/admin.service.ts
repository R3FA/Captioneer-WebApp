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

  deleteMovieSubtitle(movieSubtitleID: number, userUploader: string): Observable<HttpResponse<string>> {
    return this.http.delete(`${this.url}/SubtitleMovie/${movieSubtitleID}?userUploader=${userUploader}`, { observe: 'response', responseType: 'text' });
  }

  deleteTVShowSubtitle(subtitleId: number, userUploader: string, episodeNumber: number, seasonNumber: number): Observable<HttpResponse<string>> {
    return this.http.delete(`${this.url}/SubtitleTVShows/${subtitleId}?userUploader=${userUploader}&episodeNumber=${episodeNumber}&seasonNumber=${seasonNumber}`, { observe: 'response', responseType: 'text' });
  }
}
