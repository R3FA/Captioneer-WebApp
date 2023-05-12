import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FollowerViewModel } from '../models/follower-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class FollowerServiceService {
  private url: string = environment.apiURL + "/Follower";
  constructor(private http: HttpClient) { }

  GetFollowersOfUser(loggedUserID: number): Observable<HttpResponse<FollowerViewModel>> {
    return this.http.get<FollowerViewModel>(`${this.url}/GetFollowerCount/${loggedUserID}`, { observe: 'response' });
  }
}