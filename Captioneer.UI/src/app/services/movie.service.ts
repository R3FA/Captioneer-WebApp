import { Injectable, Input } from '@angular/core';
import { MovieViewModel } from '../models/movie-viewmodel';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  constructor(private httpClient: HttpClient) { }

  private url: string = environment.apiURL + "/Movies";

  public searchedMovies?: MovieViewModel[] = [];
  public isMovieSearched: boolean = false;

  public getMovieByParameter(movieName: string): Observable<HttpResponse<MovieViewModel[]>> {
    return this.httpClient.get<MovieViewModel[]>(`${this.url}/ ${movieName}`, { observe: 'response' });
  }

  public getMovies(): Observable<MovieViewModel[]> {
    return this.httpClient.get<MovieViewModel[]>(this.url);
  }
}
