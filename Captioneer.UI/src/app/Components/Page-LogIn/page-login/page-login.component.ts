import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { stringToKeyValue } from '@angular/flex-layout/extended/style/style-transforms';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserLogin } from 'src/app/models/user-login';
import { UserPost } from 'src/app/models/user-post';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-page-login',
  templateUrl: './page-login.component.html',
  styleUrls: ['./page-login.component.css']
})
export class PageLoginComponent implements OnInit {

  sakrij: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private loginService: UserService) {
  }

  ngOnInit() {

  }

  loginForma: FormGroup = this.fb.group({
    email: ['', Validators.required],
    password: ['', Validators.required]
  });
  onLogin() {
    if (this.loginForma.invalid)
      return;
    this.loginService.loginUser(this.loginForma.value).subscribe({
      next: (response) => {
        if(response){
          alert("You are logged in!")
          console.log(response.status)
          sessionStorage.setItem(
            'token',
            JSON.parse(JSON.stringify(response.body))['value']
            )
            sessionStorage.setItem(
              'email',
              JSON.parse(JSON.stringify((<HTMLInputElement>document.getElementById("email")).value))
              )
              window.location.href = "";
            };
          },
      error: (err) => {
        alert("Ups something went wrong")
      },
    });
  }
}