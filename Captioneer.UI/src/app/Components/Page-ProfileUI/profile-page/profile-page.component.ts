import { AfterViewInit, Component, OnInit, Renderer2 } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserPost } from 'src/app/models/user-post';
import { UserUpdate } from 'src/app/models/user-update';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { TokenValidatorService } from 'src/app/services/token-validator.service';
import { UserService } from 'src/app/services/user.service';
import { Utils } from 'src/app/utils/utils'

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent implements OnInit, AfterViewInit {

  private loggedUser! : UserViewModel | null;
  // In bytes
  private maxUploadSize = 2 * 1024 * 1024;

  private _showEditProfile!: boolean;
  private _showChangeEmailForm!: boolean;
  private _showChangeUsernameForm!: boolean;
  private _showChangePasswordForm!: boolean;
  private _showChangeProfileImageForm!: boolean;
  private _showDeleteProfileForm!: boolean;

  public changeEmailForm!: FormGroup;
  public changeUsernameForm!: FormGroup;
  public changePasswordForm!: FormGroup;
  public changeProfileImageForm!: FormGroup;
  public deleteProfileForm!: FormGroup;

  private enterPassword!: FormControl;
  private enterEmail!: FormControl;
  private enterUsername!: FormControl;
  private enterNewPassword!: FormControl;
  private enterNewPasswordRepeat!: FormControl;
  private enterNewImage!: FormControl;

  public submitted!: boolean;
  public successText!: string;
  public errorText!: string;
  public editProfileBtnText!: string;
  public fileName!: string;
  public requestInProgress!: boolean;

  public get showEditProfile(): boolean {
    return this._showEditProfile;
  }

  public get showChangeEmailForm(): boolean {
    return this._showChangeEmailForm;
  }

  public get showChangeUsernameForm(): boolean {
    return this._showChangeUsernameForm;
  }

  public get showChangePasswordForm(): boolean {
    return this._showChangePasswordForm;
  }

  public get showChangeProfileImageForm(): boolean {
    return this._showChangeProfileImageForm;
  }

  public get showDeleteProfileForm(): boolean {
    return this._showDeleteProfileForm;
  }

  constructor(private formBuilder: FormBuilder, private userService: UserService, private renderer: Renderer2, private tokenValidator : TokenValidatorService) { }

  ngOnInit(): void {

    this._showEditProfile = false;
    this._showChangeEmailForm = true;
    this._showChangeUsernameForm = false;
    this._showChangePasswordForm = false;
    this._showChangeProfileImageForm = false;
    this._showDeleteProfileForm = false;
    this.submitted = false;
    this.successText = "";
    this.errorText = "";
    this.editProfileBtnText = "Edit profile";
    this.fileName = "";
    this.requestInProgress = false;

    this.enterPassword = new FormControl('', [Validators.required]);
    this.enterEmail = new FormControl('', [Validators.required, Validators.email]);
    this.enterUsername = new FormControl('', [Validators.required, Validators.maxLength(10)]);
    this.enterNewPassword = new FormControl('', [Validators.required, Validators.minLength(6)]);
    this.enterNewPasswordRepeat = new FormControl('', [Validators.required, this.validatePasswordMatch()]);
    this.enterNewImage = new FormControl('', [Validators.required]);

    this.changeEmailForm = this.formBuilder.group({
      enterPassword: this.enterPassword,
      enterEmail: this.enterEmail
    });

    this.changeUsernameForm = this.formBuilder.group({
      enterPassword: this.enterPassword,
      enterUsername: this.enterUsername
    });

    this.changePasswordForm = this.formBuilder.group({
      enterPassword: this.enterPassword,
      enterNewPassword: this.enterNewPassword,
      enterNewPasswordRepeat: this.enterNewPasswordRepeat
    });

    this.changeProfileImageForm = this.formBuilder.group({
      enterNewImage: this.enterNewImage
    });

    this.deleteProfileForm = this.formBuilder.group({
      enterPassword: this.enterPassword
    });
  }

  async ngAfterViewInit() {
    this.loggedUser = await this.userService.getCurrentUser();

    if (!this.tokenValidator.validateToken()) {
      this.loggedUser = null;
    }
  }

  clearForms(): void {
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

  togglePublicInfo(): void {

    if (!this.loggedUser) {
      return;
    }

    this._showEditProfile = !this.showEditProfile;

    this.successText = "";
    this.errorText = "";

    this.clearForms();

    if (this._showEditProfile) {
      this.editProfileBtnText = "Back";
    }
    else {
      this.editProfileBtnText = "Edit profile";
    }
  }

  validatePasswordMatch(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isValid: boolean = this.enterNewPassword.value == control.value;

      return isValid ? null : { error: true };
    }
  }

  // Custom file validation method that bypasses Angular's form control validation
  validateFileUpload(event: any): void {
    const file: File = event.target.files[0];
    this.fileName = file.name;
    this.submitted = true;

    if (file.size == 0 || file.size > this.maxUploadSize) {
      this.enterNewImage.setErrors({ error: true });
      this.fileName = "";
    }
    else if (file.type != "image/jpeg" && file.type != "image/png") {
      this.enterNewImage.setErrors({ error: true });
      this.fileName = "";
    }
    else {
      this.enterNewImage.setErrors(null);
      this.changeProfileImage(file);
    }
  }

  toggleForm(event: any): void {

    var input = event.target as HTMLInputElement;

    if (input.id == "changeEmailBtn") {
      this._showChangeEmailForm = !this._showChangeEmailForm;
      this._showChangePasswordForm = false;
      this._showChangeUsernameForm = false;
      this._showChangeProfileImageForm = false;
      this._showDeleteProfileForm = false;
    }
    else if (input.id == "changeUsernameBtn") {
      this._showChangeUsernameForm = !this._showChangeUsernameForm;
      this._showChangePasswordForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showDeleteProfileForm = false;
    }
    else if (input.id == "changePasswordBtn") {
      this._showChangePasswordForm = !this._showChangePasswordForm;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showDeleteProfileForm = false;
    }
    else if (input.id == "changeProfileImageBtn") {
      this._showChangeProfileImageForm = !this._showChangeProfileImageForm;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangePasswordForm = false;
      this._showDeleteProfileForm = false;
    }
    else if (input.id == "deleteProfileBtn") {
      this._showDeleteProfileForm = true;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showChangePasswordForm = false;
    }

    this.fileName = "";
    this.clearForms();
  }

  changeEmail(): void {
    this.submitted = true;

    if (this.changeEmailForm.invalid) {
      return;
    }

    this.requestInProgress = true;

    var userUpdate: UserUpdate = {
      password: this.enterPassword.value,
      newEmail: this.enterEmail.value
    }

    this.userService.putUser(userUpdate, this.loggedUser!.username).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed email"
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  changeUsername(): void {
    this.submitted = true;

    if (this.changeUsernameForm.invalid) {
      return;
    }

    this.requestInProgress = true;

    var userUpdate: UserUpdate = {
      password: this.enterPassword.value,
      newUsername: this.enterUsername.value
    }

    this.userService.putUser(userUpdate, this.loggedUser!.username).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed username";
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  changePassword(): void {
    this.submitted = true;

    if (this.changePasswordForm.invalid) {
      return;
    }

    this.requestInProgress = true;

    var userUpdate: UserUpdate = {
      password: this.enterPassword.value,
      newPassword: this.enterPassword.value
    }

    this.userService.putUser(userUpdate, this.loggedUser!.username).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed password";
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  async changeProfileImage(file: File): Promise<void> {
    this.submitted = true;

    if (this.changeProfileImageForm.invalid) {
      return;
    }

    this.requestInProgress = true;

    var userUpdate: UserUpdate = {
      newProfileImage: await Utils.ToBase64(file)
    };

    this.userService.putUser(userUpdate, this.loggedUser!.username).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed profile image";
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  deleteProfile(): void {
    this.submitted = true;

    if (this.deleteProfileForm.invalid) {
      return;
    }

    this.requestInProgress = true;

    var userPost: UserPost = {
      password: this.enterPassword.value,
      username: this.loggedUser!.username,
      email: this.loggedUser!.email,
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
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  private reloadPage() : void {
    setTimeout(() => {
      window.location.reload();
    }, 1500);
  }
}
