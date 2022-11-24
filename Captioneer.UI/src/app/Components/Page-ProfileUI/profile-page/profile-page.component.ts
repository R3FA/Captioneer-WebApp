import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserPost } from 'src/app/models/user-post';
import { UserUpdate } from 'src/app/models/user-update';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent implements OnInit, AfterViewInit {

  private _showEditProfile! : boolean;
  private _showChangeEmailForm! : boolean;
  private _showChangeUsernameForm! : boolean;
  private _showChangeProfileImageForm! : boolean;
  private _showChangePasswordForm! : boolean;
  private _showDeleteProfileForm! : boolean;

  public changeEmailForm! : FormGroup;
  public changeUsernameForm! : FormGroup;
  public changePasswordForm! : FormGroup;
  public deleteProfileForm! : FormGroup;

  private enterPassword! : FormControl;
  private enterEmail! : FormControl;
  private enterUsername! : FormControl;
  private enterNewPassword! : FormControl;
  private enterNewPasswordRepeat! : FormControl;

  public submitted! : boolean;
  public successText! : string;
  public errorText! : string;
  public editProfileBtnText! : string;

  public get showEditProfile() : boolean {
    return this._showEditProfile;
  }

  public get showChangeEmailForm() : boolean {
    return this._showChangeEmailForm;
  }
  
  public get showChangeUsernameForm() : boolean {
    return this._showChangeUsernameForm;
  }

  public get showChangeProfileImageForm() : boolean {
    return this._showChangeProfileImageForm;
  }
  
  public get showChangePasswordForm() : boolean {
    return this._showChangePasswordForm;
  }

  public get showDeleteProfileForm() : boolean {
    return this._showDeleteProfileForm;
  }
  
  
  constructor(private formBuilder : FormBuilder, private userService : UserService) {}
  
  ngOnInit(): void {
    this._showEditProfile = true;
    this._showChangeEmailForm = true;
    this._showChangePasswordForm = false;
    this._showChangeProfileImageForm = false;
    this._showChangeUsernameForm = false;
    this._showDeleteProfileForm = false;
    this.submitted = false;
    this.successText = "";
    this.errorText = "";
    this.editProfileBtnText = "Edit profile";

    this.enterPassword = new FormControl('', [Validators.required]);
    this.enterEmail = new FormControl('', [Validators.required, Validators.email]);
    this.enterUsername = new FormControl('', [Validators.required, Validators.maxLength(10)]);
    this.enterNewPassword = new FormControl('', [Validators.required, Validators.minLength(6)]);
    this.enterNewPasswordRepeat = new FormControl('', [Validators.required, this.validatePasswordMatch()]);

    this.changeEmailForm = this.formBuilder.group({
      enterPassword : this.enterPassword,
      enterEmail : this.enterEmail
    })

    this.changeUsernameForm = this.formBuilder.group({
      enterPassword : this.enterPassword,
      enterUsername : this.enterUsername
    })

    this.changePasswordForm = this.formBuilder.group({
      enterPassword : this.enterPassword,
      enterNewPassword : this.enterNewPassword,
      enterNewPasswordRepeat : this.enterNewPasswordRepeat
    })

    this.deleteProfileForm = this.formBuilder.group({
      enterPassword : this.enterPassword
    })
  }
  
  ngAfterViewInit() {

  }

  clearForms() : void {
    this.changeEmailForm.reset();
    this.changeUsernameForm.reset();
    this.changePasswordForm.reset();
    this.deleteProfileForm.reset();

    this.enterEmail.setErrors(null);
    this.enterUsername.setErrors(null);
    this.enterPassword.setErrors(null);
    this.enterNewPassword.setErrors(null);
    this.enterNewPasswordRepeat.setErrors(null);
  }

  togglePublicInfo() : void {
    this._showEditProfile = !this.showEditProfile;

    this.successText = "";
    this.errorText = "";

    this.clearForms();

    if (this._showEditProfile)  {
      this.editProfileBtnText = "Back";
    }
    else {
      this.editProfileBtnText = "Edit profile";
    }
  }

  validatePasswordMatch() : ValidatorFn {
    return (control : AbstractControl) : ValidationErrors | null => {
      const isValid : boolean = this.enterNewPassword.value == control.value;

      return isValid? null : { error : true }
    }
  }

  toggleForm(event : any) : void {

    var input = event.target as HTMLInputElement;

    if (input.id == "changeEmailBtn")  {
      this._showChangeEmailForm = !this._showChangeEmailForm;
      this._showChangePasswordForm = false;
      this._showChangeUsernameForm = false;
      this._showDeleteProfileForm = false;
      this.clearForms();
    }
    else if (input.id == "changeUsernameBtn")  {
      this._showChangeUsernameForm = !this._showChangeUsernameForm;
      this._showChangePasswordForm = false;
      this._showChangeEmailForm = false;
      this._showDeleteProfileForm = false;
      this.clearForms();
    }
    else if (input.id == "changePasswordBtn")  {
      this._showChangePasswordForm = !this._showChangePasswordForm;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showDeleteProfileForm = false;
      this.clearForms();
    }
    else if (input.id == "deleteProfileBtn")  {
      this._showDeleteProfileForm = true;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangePasswordForm = false;
      this.clearForms();
    }
  }

  changeEmail() : void  {
    this.submitted = true;

    if (this.changeEmailForm.invalid) {
      return;
    }

    var userUpdate : UserUpdate = {
      password: this.enterPassword.value,
      newEmail: this.enterEmail.value
    }

    // Test username. Replace with actual username of logged user when implemented
    this.userService.putUser(userUpdate, "newuser").subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed email"
        this.errorText = "";
      }
    });
  }

  changeUsername() : void {
    this.submitted = true;

    if (this.changeUsernameForm.invalid) {
      return;
    }

    var userUpdate : UserUpdate = {
      password: this.enterPassword.value,
      newUsername: this.enterUsername.value
    }

    this.userService.putUser(userUpdate, "newuser").subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed username";
        this.errorText = "";
      }
    });
  }

  changePassword() : void {
    this.submitted = true;

    if (this.changePasswordForm.invalid) {
      return;
    }

    var userUpdate : UserUpdate = {
      password: this.enterPassword.value,
      newPassword: this.enterPassword.value
    }

    this.userService.putUser(userUpdate, "newuser").subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed password";
        this.errorText = "";
      }
    });
  }

  deleteProfile() : void {
    this.submitted = true;

    if (this.deleteProfileForm.invalid) {
      return;
    }

    // Test data for user. Replace with actual logged in user when implemented
    var userPost : UserPost = {
      password: this.enterPassword.value,
      username: "newuser",
      email: "newuser",
    }

    this.userService.deleteUser(userPost).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Account has been succesfully deleted"
        this.errorText = "";
      }
    });
  }
}
