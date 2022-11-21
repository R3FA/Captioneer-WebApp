import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { PageLoginComponent } from './Components/Page-LogIn/page-login/page-login.component';
import { SignupPageComponent } from './Components/signup-page/signup-page.component';;

const routes: Routes = [
  {
    path:'signup',
    component:SignupPageComponent
  },
  {
    path:'',
    component:PageLoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule
{

}