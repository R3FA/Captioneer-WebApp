import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class PaginationService {

  constructor(private httpService:HttpClient) { }

  getData()
  {
    return this.httpService.get(environment.apiURL + "/Movies");
  }
}
