import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserLanguageModel } from '../models/userLanguage-viewmodel';

@Injectable({
  providedIn: 'root'
})
export class UserlanguageService {

  constructor(private httpClient: HttpClient) { }
  private url: string = environment.apiURL + "/UserLanguages";

  public getUserLanguage(userName: string): Observable<UserLanguageModel[]> {
    return this.httpClient.get<UserLanguageModel[]>(`${this.url}/${userName}`);
  }

  public postUserLanguage(userName: string, engLanguage: string): Observable<UserLanguageModel> {
    return this.httpClient.post<UserLanguageModel>(`${this.url}/${userName}?englishLanguageName=${engLanguage}`, { observe: 'response' });
  }

  public deleteUserLanguage(userName: string, engLanguage: string): Observable<UserLanguageModel> {
    return this.httpClient.delete<UserLanguageModel>(`${this.url}/${userName}?englishLanguageName=${engLanguage}`);
  }
}
