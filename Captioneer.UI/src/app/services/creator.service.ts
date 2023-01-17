import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Creator } from '../models/creator';
import { Language } from '../models/language';

@Injectable({
  providedIn: 'root'
})
export class CreatorService {

  constructor(private httpClient: HttpClient) { }

  private urlMovie: string=environment.apiURL+"/CreatorMovies";
  private urlTVShow: string=environment.apiURL+"/CreatorTVShows";

  public getCreators(isTVShow:boolean,id:string):Observable<Creator[]>{
    if(isTVShow)
    {
      return this.httpClient.get<Creator[]>(`${this.urlTVShow+"/"+id}`);
    }
    else{
      return this.httpClient.get<Creator[]>(`${this.urlMovie+"/"+id}`);
    }
  }
}
