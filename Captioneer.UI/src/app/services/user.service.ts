import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserPost } from '../models/user-post';
import { UserUpdate } from '../models/user-update'
import { UserViewModel } from '../models/user-viewmodel';
import { UserLogin } from '../models/user-login';
import { firstValueFrom } from 'rxjs';import { Utils } from '../utils/utils';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url: string = environment.apiURL + "/Users"

  public userData?:any;

  constructor(private httpClient: HttpClient) { }

  getUser(): Observable<HttpResponse<UserViewModel>> {
    return this.httpClient.get<UserViewModel>(this.url, { observe: 'response' });
  }
  getUserByEmail(email?: string,username?:string): Observable<HttpResponse<UserViewModel>> {
    return this.httpClient.get<UserViewModel>(this.url + '?mail=' + email + "&username=" + username, { observe: 'response' });
  }

  postUser(user: UserPost): Observable<HttpResponse<UserPost>> {
    return this.httpClient.post<UserPost>(this.url, user, { observe: 'response' });
  }

  loginUser(user: UserLogin): Observable<HttpResponse<UserLogin>> {
    return this.httpClient.post<UserLogin>(`${this.url}/login`, user, { observe: 'response' });
  }

  putUser(user: UserUpdate, username: string): Observable<HttpResponse<UserUpdate>> {
    return this.httpClient.put<UserUpdate>(this.url + `/${username}`, user, { observe: 'response' });
  }

  deleteUser(user: UserPost): Observable<HttpResponse<UserPost>> {
    return this.httpClient.delete<UserPost>(this.url, { observe: 'response', body: user });
  }

  async getCurrentUser(): Promise<UserViewModel | null> {

    return new Promise((resolve) => {

      var email = window.sessionStorage.getItem("email");
  
      if (!email) {
        resolve(null);
      }

      this.getUserByEmail(email!).subscribe({
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

  async getUserProfileImage(username : string) : Promise<string | null> {
    return new Promise((resolved : any) => {
      this.httpClient.get(`${this.url}/${username}/profileimage`, { responseType: "text" }).subscribe({
        next: (response) => {
          resolved(response);
        },
        error: (err) => {
          console.error(err.error);
          resolved(null);
        }
      })
    })
  }
}
