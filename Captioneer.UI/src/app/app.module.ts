import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SignupPageComponent } from './Components/signup-page/signup-page.component';
import { PageLoginComponent } from './Components/Page-LogIn/page-login/page-login.component';
import { HeaderComponentComponent } from './Components/Main-Header/header-component/header-component.component';
import { FooterComponentComponent } from './Components/Main-Footer/footer-component/footer-component.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';
import { SignUpDoneComponent } from './Components/sign-up-done/sign-up-done.component';
import { ProfilePageComponent } from './Components/Page-ProfileUI/profile-page/profile-page.component';
import { FooterV2Component } from './Components/Main-Footer-v2/footer-v2/footer-v2.component';
import { HomepageComponent } from './Components/homepage/homepage.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FavoritesGraphicsComponent } from './Components/favorites-graphics/favorites-graphics.component';
import { MovieInfoComponent } from './Components/movie-info/movie-info.component';
import { NgxStarRatingModule } from 'ngx-star-rating';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { PageCommentComponent } from './Components/Page-Comments/page-comment/page-comment.component';
import { PageDirectMessagesComponent } from './Components/page-direct-messages/page-direct-messages.component';
@NgModule({
  declarations: [
    AppComponent,
    SignupPageComponent,
    PageLoginComponent,
    HeaderComponentComponent,
    FooterComponentComponent,
    SignUpDoneComponent,
    ProfilePageComponent,
    FooterV2Component,
    HomepageComponent,
    FavoritesGraphicsComponent,
    MovieInfoComponent,
    PageCommentComponent,
    PageDirectMessagesComponent,
  ],
  imports: [
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule,
    FlexLayoutModule,
    HttpClientModule,
    MatDialogModule,
    NgxPaginationModule,
    MatAutocompleteModule,
    NgxStarRatingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function httpTranslateLoader(http : HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n', '.json');
}