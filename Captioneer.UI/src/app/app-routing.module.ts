import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { LoginPageComponent } from './Components/login-page/login-page.component'
import { SignupPageComponent } from './signup-page/signup-page.component';;

const routes: Routes = [
  {
    path:'signup',
    component:SignupPageComponent
  },
  {
    path: '',
    component: AppComponent,
    children:
      [
        {
          path: '',
          component: LoginPageComponent
        }
      ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule
{

}