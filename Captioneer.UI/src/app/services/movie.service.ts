import { Injectable, Input } from '@angular/core';
import { Movies } from '../models/movies';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  constructor(private httpClient: HttpClient) { }

  private url: string = environment.apiURL + "/Movies";

  public getMovieByParameter(movieName: string): Observable<HttpResponse<Movies[]>> {
    return this.httpClient.get<Movies[]>(`${this.url}/ ${movieName}`, { observe: 'response' });
  }

  public getMovies(): Observable<Movies[]> {
    return this.httpClient.get<Movies[]>(this.url);
  }
}
