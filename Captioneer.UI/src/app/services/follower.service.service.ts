import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FollowerViewModel } from '../models/follower-viewmodel';
import { UserViewModel } from '../models/user-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class FollowerServiceService {
  private url: string = environment.apiURL + "/Follower";
  constructor(private http: HttpClient) { }

  GetFollowersOfUser(loggedUserID: number): Promise<FollowerViewModel> {
    return new Promise((resolve, reject) => {
      this.http.get<FollowerViewModel>(`${this.url}/GetFollowerCount/${loggedUserID}`).subscribe(response => {
        resolve(response);
      }, error => {
        reject(error);
      })
    });
  }

  PostFollower(loggedUser: UserViewModel, friendsUsername: string): Observable<HttpResponse<string>> {
    return this.http.post(`${this.url}/AddAUserFollower?followerUsername=${friendsUsername}`, loggedUser, { observe: 'response', responseType: 'text' });
  }

  DeleteFollower(loggedUser: UserViewModel, friendsUsername: string): Observable<HttpResponse<string>> {
    return this.http.delete(`${this.url}/DeleteFollower?followerUsername=${friendsUsername}`, { body: loggedUser, observe: 'response', responseType: 'text' });
  }
}