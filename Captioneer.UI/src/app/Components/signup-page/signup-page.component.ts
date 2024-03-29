import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { SignUpDoneComponent } from '../sign-up-done/sign-up-done.component';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-signup-page',
  templateUrl: './signup-page.component.html',
  styleUrls: ['./signup-page.component.css']
})
export class SignupPageComponent implements OnInit {
  hide: boolean = false;
  User={
    Email:'',
    Username:'',
    Password:''
  };
  errorMessage!: string;
  constructor(private fb: FormBuilder,private http:HttpClient,private dialogRef:MatDialog, private userService : UserService) { 
  }

  ngOnInit(): void { }
  SignupForm: FormGroup = this.fb.group(
    {
      email: new FormControl('', [Validators.required, Validators.email]),
      text: new FormControl('', [Validators.required, Validators.required]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    }
  );
  
    onSignup():void {
      console.warn('You have signed up !', this.SignupForm.value) 
      this.User.Email=this.SignupForm.value.email
      this.User.Username=this.SignupForm.value.text
      this.User.Password=this.SignupForm.value.password
      this.http.post<any>(environment.apiURL + "/Users", this.User).subscribe((data=>{
          console.log(data)
          this.dialogRef.open(SignUpDoneComponent);
        }),
        (err)=>{
          this.errorMessage=err.error;
        }
        )
    // this.http.get("https://localhost:7207/api/Movies").subscribe((data=>{
    //   console.log(data)
    // }))
    }
}
