import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SignupPageComponent } from './Components/signup-page/signup-page.component';
import { RouterModule, Routes } from '@angular/router';
import { PageLoginComponent } from './Components/Page-LogIn/page-login/page-login.component';
import { HeaderComponentComponent } from './Components/Main-Header/header-component/header-component.component';
import { FooterComponentComponent } from './Components/Main-Footer/footer-component/footer-component.component';
import { HttpClientModule } from '@angular/common/http';
import{MatDialogModule} from '@angular/material/dialog';
import { SignUpDoneComponent } from './Components/sign-up-done/sign-up-done.component';

const routes:Routes=[
{path:'signup',component:SignupPageComponent}
];

@NgModule({
  declarations: [
    AppComponent,
    SignupPageComponent,
    PageLoginComponent,
    HeaderComponentComponent,
    FooterComponentComponent,
    SignUpDoneComponent,
  ],
  imports: [
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
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }