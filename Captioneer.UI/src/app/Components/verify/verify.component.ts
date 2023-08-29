import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { UserViewModel } from 'src/app/models/user-viewmodel';
// import { FormControl, FormGroup, FormsModule } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.css']
})
export class VerifyComponent implements OnInit {

  constructor(public userService: UserService, private route: Router) { }

  private userCredentials!: FormGroup;

  public loggingUser!: UserViewModel;
  public verifyCode: string = '';
  public isMailSent: boolean = false;


  async ngOnInit(): Promise<void> {
    this.loggingUser = await new Promise((resolved) => {
      this.userService.getUserByEmail(this.userService.verificationUserEmail).subscribe({
        next: (response) => {
          if (response.body != null) {
            resolved(response.body);
          }
        },
        error: (err) => { console.log(err); }
      });
    });
    this.userCredentials = this.userService.verificationLoginForm;
  }

  async sendEmail() {
    this.isMailSent = true;
    this.userService.sendVerificationMail(this.loggingUser.id).subscribe({
      next: () => { alert("Email vam je uspjesno poslan!"); },
      error: (err) => { console.log(err); }
    })
  }

  async Verify() {
    this.userService.verifyUser(this.loggingUser.id, this.verifyCode).subscribe({
      next: () => {
        console.log("Uspjesno verifikovan!");

        this.userService.loginUser(this.userCredentials.value).subscribe({
          next: (response) => {
            if (response) {
              alert("You are logged in!");
              sessionStorage.setItem(
                'token',
                JSON.parse(JSON.stringify(response.body))['value']
              );
              sessionStorage.setItem(
                'email',
                JSON.parse(JSON.stringify(this.loggingUser.email))
              );
              this.route.navigate(['/Home']);
            };
          },
          error: (err) => {
            alert("Ups something went wrong")
          },
        });

      },
      error: (err) => { console.log(err); }
    });
  }
}