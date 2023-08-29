import { Component, NgModule } from '@angular/core';
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
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundErrorComponent } from './Components/page-not-found-error/page-not-found-error.component';
import { FollowFriendsComponent } from './Components/Page-ProfileUI/profile-page/follow-friends/follow-friends.component';
import { AngularFireModule } from '@angular/fire/compat';
import { AngularFireAuthModule } from '@angular/fire/compat/auth';
import { environment } from '../environments/environment';
import { UserSearchComponent } from './Components/Page-ProfileUI/profile-page/user-search/user-search.component';
import { VerifyComponent } from './Components/verify/verify.component';


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
    PageNotFoundErrorComponent,
    FollowFriendsComponent,
    UserSearchComponent,
    VerifyComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
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
    AngularFireAuthModule,
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

export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}