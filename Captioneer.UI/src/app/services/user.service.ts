import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserPost } from '../models/user-post';
import { UserUpdate } from '../models/user-update'
import { UserViewModel } from '../models/user-viewmodel';
import { UserLogin } from '../models/user-login';
import { firstValueFrom } from 'rxjs'; import { Utils } from '../utils/utils';
import { UserResponse } from '../models/userresponse-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url: string = environment.apiURL + "/Users";
  public userData!: any;
  public userID!: number;
  public addFriendComponentClicked = false;
  public userSearchComponentClicked = false;


  constructor(private httpClient: HttpClient) { }

  getUser(pageNumber: number): Observable<HttpResponse<UserResponse>> {
    return this.httpClient.get<UserResponse>(`${this.url}/GetAllUsers?page=${pageNumber}`, { observe: 'response' });
  }
  getUserByEmail(email?: string, username?: string): Observable<HttpResponse<UserViewModel>> {
    return this.httpClient.get<UserViewModel>(this.url + '?mail=' + email + "&username=" + username, { observe: 'response' });
  }

  getUserByID(id: number): Observable<HttpResponse<UserViewModel>> {
    return this.httpClient.get<UserViewModel>(`${this.url}/${id}`, { observe: 'response' });
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
  banUser(adminUser: UserViewModel | null, userID: number): Observable<HttpResponse<string>> {
    return this.httpClient.put(this.url + '/BanUser/' + userID, adminUser, { observe: 'response', responseType: 'text' });
  }
  // localhost:7207/api/Users/BanUser/74

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

  async getUserProfileImage(username: string): Promise<string | null> {
    return new Promise((resolved: any) => {
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
