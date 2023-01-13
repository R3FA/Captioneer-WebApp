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

  get(username : string) : Observable<HttpResponse<MovieViewModel[]>> {
    return this.httpClient.get<MovieViewModel[]>(this.url + `/${username}`, {observe: 'response'});
  }

  post(username : string, movie : MovieViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.post<void>(this.url + `/${username}`, movie, {observe: 'response'});
  }

  delete(username : string, movie : MovieViewModel) : Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.url + `/${username}`, {body: movie, observe: 'response'});
  }

  getFavoriteMovies(username : string) : Promise<MovieViewModel[] | null> {
    
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
