import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class PaginationService {

  constructor(private httpService:HttpClient) { }

  getMovieData(pageNumber:any)
  {
    return this.httpService.get(environment.apiURL + "/Movies?page="+pageNumber+"&pageSize=6");
  }
  getTVShowData(pageNumber:any)
  {
    return this.httpService.get(environment.apiURL + "/TVShows?page="+pageNumber+"&pageSize=6");
  }
}
