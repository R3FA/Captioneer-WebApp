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

  getFavoriteTVShows(username : string) : Observable<HttpResponse<TVShowViewModel[]>> {
    return this.httpClient.get<TVShowViewModel[]>(this.url + username, {observe: 'response'});
  }

  postFavoriteTVShow(username : string, tvShow : TVShowViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.post<void>(this.url + username, tvShow, {observe: 'response'});
  }

  deleteFavoriteTVShow(username : string, tvShow : TVShowViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.url + username, {body: tvShow, observe: 'response'});
  }
}
