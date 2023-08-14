import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TVShowViewModel } from '../models/tvshow-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class TvshowService {

  private url: string = environment.apiURL + '/TVShows';

  public searchedTVShows?: TVShowViewModel[] = [];
  public isTVShowSearched: boolean = false;

  constructor(private http: HttpClient) { }

  public GetTVShow(tvShowName: string): Observable<HttpResponse<TVShowViewModel[]>> {
    return this.http.get<TVShowViewModel[]>(`${this.url}/${tvShowName}`, { observe: 'response' });
  }
}
