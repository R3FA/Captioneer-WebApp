import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class TokenValidatorService {

  constructor(private httpClient: HttpClient) { }

  validateToken(){
    var validation;
    this.httpClient.get<Boolean>(environment.apiURL+"/"+"TokenControler"+"/"+window.sessionStorage.getItem('token')).subscribe((data)=>{
     window.sessionStorage.setItem('tokenValid',JSON.stringify(data))
  });
  if(JSON.parse(window.sessionStorage.getItem('tokenValid')!))
    return true;
  return false;
  }
}
