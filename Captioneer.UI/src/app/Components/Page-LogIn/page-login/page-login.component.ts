import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { stringToKeyValue } from '@angular/flex-layout/extended/style/style-transforms';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserLogin } from 'src/app/models/user-login';
import { UserPost } from 'src/app/models/user-post';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-page-login',
  templateUrl: './page-login.component.html',
  styleUrls: ['./page-login.component.css']
})
export class PageLoginComponent implements OnInit {

  sakrij: boolean = false;
  private userEmail: any = "";
  private user: any;
  private stopLoginProcess!: boolean;

  constructor(private fb: FormBuilder, private http: HttpClient, private loginService: UserService, private router: Router) {
  }

  ngOnInit() {

  }

  loginForma: FormGroup = this.fb.group({
    email: ['', Validators.required],
    password: ['', Validators.required]
  });
  async onLogin(): Promise<any> {
    if (this.loginForma.invalid)
      return;
    this.userEmail = this.loginForma.controls['email'].value;
    this.loginService.getUserByEmail(this.userEmail).subscribe((data) => {
      this.user = data.body
      if (this.user.isBanned == true)
        this.stopLoginProcess = true;
      else
        this.stopLoginProcess = false;

      if (!this.stopLoginProcess) {
        // ODAVDJE
        this.loginService.verificationUserEmail = this.loginForma.controls['email'].value;
        this.loginService.verificationLoginForm = this.loginForma;

        if (this.user.isVerificationActive) {
          this.router.navigate(['/Verify']);
        } else {
          this.loginService.loginUser(this.loginForma.value).subscribe({
            next: (response) => {
              if (response) {
                // console.log(response.body);
                alert("You are logged in!")
                // console.log(response.status)
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
      else { alert("This account is banned and you can't log into!"); window.location.href = "/Signin"; }
    });
  }
}