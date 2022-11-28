import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class PaginationService {

  constructor(private httpService:HttpClient) { }

  getData()
  {
    return this.httpService.get("https://localhost:7207/api/Movies");
  }
}
