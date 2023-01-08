import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MovieViewModel } from '../models/movie-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class FavoriteMoviesService {

  private url : string = environment.apiURL + "/UserMovies"

  constructor(private httpClient : HttpClient) { }

  getFavoriteMovies(username : string) : Observable<HttpResponse<MovieViewModel[]>> {
    return this.httpClient.get<MovieViewModel[]>(this.url + `/${username}`, {observe: 'response'});
  }

  postFavoriteMovie(username : string, movie : MovieViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.post<void>(this.url + `/${username}`, movie, {observe: 'response'});
  }

  deleteFavoriteMovie(username : string, movie : MovieViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.url + `/${username}`, {body: movie, observe: 'response'});
  }
}
