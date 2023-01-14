import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PageLoginComponent } from './Components/Page-LogIn/page-login/page-login.component';
import { SignupPageComponent } from './Components/signup-page/signup-page.component';
import { ProfilePageComponent } from './Components/Page-ProfileUI/profile-page/profile-page.component';
import { PageCommentComponent } from './Components/Page-Comments/page-comment/page-comment.component';
import { HomepageComponent } from './Components/homepage/homepage.component';
import { MovieInfoComponent } from './Components/movie-info/movie-info.component';
import { PageDirectMessagesComponent } from './Components/page-direct-messages/page-direct-messages.component';
const routes: Routes = [
  {
    path: 'home',
    component: HomepageComponent
  },
  {
    path: 'profile-page',
    component: ProfilePageComponent
  },
  {
    path: 'signup',
    component: SignupPageComponent
  },
  {
    path: '',
    component: PageLoginComponent
  },
  {
    path: 'movie',
    component: MovieInfoComponent
  },
  {
    path: 'comments',
    component: PageCommentComponent
  },
  {
    path: 'directmessages',
    component: PageDirectMessagesComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule {

}