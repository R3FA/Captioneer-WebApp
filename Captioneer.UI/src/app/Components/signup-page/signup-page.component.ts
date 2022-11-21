import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms'

@Component({
  selector: 'app-signup-page',
  templateUrl: './signup-page.component.html',
  styleUrls: ['./signup-page.component.css']
})
export class SignupPageComponent implements OnInit {
  hide: boolean = false;
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void { }
  loginForm: FormGroup = this.fb.group(
    {
      email: new FormControl('', [Validators.required, Validators.email]),
      text: new FormControl('', [Validators.required, Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(12)])
    }
  );
  
    onLogin() {
  
    }
  
}
