import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as saveAs from 'file-saver';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Language } from '../models/language';
import { TranslationPostModel } from '../models/translation-post';

@Injectable({
  providedIn: 'root'
})
export class SubtitletranslationService {

  constructor(private httpClient : HttpClient) { }

  get() : Observable<HttpResponse<Language[]>> {
    return this.httpClient.get<Language[]>(`${environment.apiURL}/Translation`, {observe: 'response'});
  }

  post(model : TranslationPostModel ) : Observable<HttpResponse<Blob>> {
    return this.httpClient.post(`${environment.apiURL}/Translation`, model, { observe: 'response', responseType: 'blob' })
  }

  getTranslatableLanguages() : Promise<Language[] | null> {
    return new Promise((resolve) => {
      this.get().subscribe({
        next: (response) => {
          resolve(response.body);
        },
        error: (err) => {
          console.error(err);
          resolve(null);
        }
      })
    })
  }

  translate(model : TranslationPostModel) : Promise<void> {
    return new Promise((resolve) => {
      this.post(model).subscribe({
        next: (response) => {
          if (response.body)  {
            saveAs(response.body, `${model.release}.srt`);
          }
        },
        error: (err) => {
          console.error(err);
          resolve();
        },
        complete: () => {
          resolve();
        }
      })
    })
  }
}
