import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms'

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {

  hide: boolean = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void { }
  loginForm: FormGroup = this.fb.group(
    {
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(12)])
    }
  );

  onLogin() {

  }
}
