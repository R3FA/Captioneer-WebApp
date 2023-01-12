import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Language } from '../models/language';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  constructor(private httpClient: HttpClient) { }

  private url: string = environment.apiURL + "/Languages";

  public getAllLanguages(): Observable<Language[]> {
    return this.httpClient.get<Language[]>(`${this.url}`);
  }
}
