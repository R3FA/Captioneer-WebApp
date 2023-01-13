import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TVShowViewModel } from '../models/tvshow-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class FavoriteTVShowsService {

  private url : string = environment.apiURL + "/UserTVShows"

  constructor(private httpClient : HttpClient) { }

  get(username : string) : Observable<HttpResponse<TVShowViewModel[]>> {
    return this.httpClient.get<TVShowViewModel[]>(this.url + `/${username}`, {observe: 'response'});
  }

  post(username : string, tvShow : TVShowViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.post<void>(this.url + `/${username}`, tvShow, {observe: 'response'});
  }

  delete(username : string, tvShow : TVShowViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.url + `/${username}`, {body: tvShow, observe: 'response'});
  }

  getFavoriteShows(username : string) : Promise<TVShowViewModel[] | null> {
    
    return new Promise((resolve) => {
      this.get(username).subscribe({
        next: (response) => {
          resolve(response.body);
        },
        error: (err) => {
          console.error(err);
          resolve(null);
        }
      })
    });
  }
}
