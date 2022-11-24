import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserPost } from '../models/user-post';
import { UserUpdate } from '../models/user-update'
import { UserViewModel } from '../models/user-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url : string = environment.apiURL + "/Users"

  constructor(private httpClient : HttpClient) { }

  getUser() : Observable<HttpResponse<UserViewModel>> {
    return this.httpClient.get<UserViewModel>(this.url, {observe: 'response'});
  }

  postUser(user : UserPost) : Observable<HttpResponse<UserPost>>  {
    return this.httpClient.post<UserPost>(this.url, user, {observe: 'response'});
  }

  putUser(user : UserUpdate, username : string) : Observable<HttpResponse<UserUpdate>> {
    return this.httpClient.put<UserUpdate>(this.url + `/${username}`, user, {observe: 'response'});
  }

  deleteUser(user : UserPost) : Observable<HttpResponse<UserPost>> {
    return this.httpClient.delete<UserPost>(this.url, {observe: 'response', body: user});
  }
}
