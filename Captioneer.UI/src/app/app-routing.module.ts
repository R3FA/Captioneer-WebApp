import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PageLoginComponent } from './Components/Page-LogIn/page-login/page-login.component';
import { SignupPageComponent } from './Components/signup-page/signup-page.component';
import { ProfilePageComponent } from './Components/Page-ProfileUI/profile-page/profile-page.component';

import { HomepageComponent } from './Components/homepage/homepage.component';
import { MovieInfoComponent } from './Components/movie-info/movie-info.component';
const routes: Routes = [
  {
    path: '',
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
    path: 'signin',
    component: PageLoginComponent
  },
  {
    path: 'movie',
    component: MovieInfoComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule {

}