import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DirectMessageViewModel } from '../models/directmessage-viewmodel';
import { Observable } from 'rxjs/internal/Observable';
import { UserViewModel } from '../models/user-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class DirectmessageService {
  private url: string = environment.apiURL + "/DirectMessage";
  constructor(private http: HttpClient) { }

  GetAllConversations(userID: number | undefined): Observable<HttpResponse<UserViewModel[]>> {
    return this.http.get<UserViewModel[]>(`${this.url}/GetAllConversations/${userID}`, { observe: 'response' });
  }

  GetMessagesForUser(senderID: number | undefined, receiverID: number | undefined): Observable<HttpResponse<DirectMessageViewModel[]>> {
    return this.http.get<DirectMessageViewModel[]>(`${this.url}/GetMessagesForUser/${senderID}/${receiverID}`, { observe: 'response' });
  }

  SendMessage(objectData: DirectMessageViewModel): Observable<HttpResponse<DirectMessageViewModel>> {
    return this.http.post<DirectMessageViewModel>(`${this.url}/SendMessage`, objectData, { observe: 'response' });
  }
}
