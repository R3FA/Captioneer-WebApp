import { AfterViewInit, Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, ParamMap, Params } from '@angular/router';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';
import { UserPost } from 'src/app/models/user-post';
import { UserUpdate } from 'src/app/models/user-update';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { TokenValidatorService } from 'src/app/services/token-validator.service';
import { UserService } from 'src/app/services/user.service';
import { Language } from 'src/app/models/language';
import { LanguageService } from 'src/app/services/language.service';
import { UserlanguageService } from 'src/app/services/userlanguage.service';
import { UserLanguageModel } from 'src/app/models/userLanguage-viewmodel';
import { Utils } from 'src/app/utils/utils';
import { environment } from 'src/environments/environment';
import { FollowerServiceService } from 'src/app/services/follower.service.service';
import { FollowerViewModel } from 'src/app/models/follower-viewmodel';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent implements OnInit, AfterViewInit {

  public loggedUser!: UserViewModel | null;
  public selectedUser: UserViewModel | null = new UserViewModel();
  public followCount: FollowerViewModel = new FollowerViewModel();
  public currentID: number = 0;
  public hiddenParentComponent: boolean = false;
  public shouldLoadFollowCount: boolean = false;
  public shouldLoad: boolean = false;
  // In bytes
  private maxUploadSize = 2 * 1024 * 1024;
  public getLanguages: Language[] = [];
  public serverPictureLocation = environment.baseAPIURL;

  private _showEditProfile!: boolean;
  private _showChangeEmailForm!: boolean;
  private _showChangeUsernameForm!: boolean;
  private _showChangePasswordForm!: boolean;
  private _showChangeProfileImageForm!: boolean;
  private _showDeleteProfileForm!: boolean;
  private _showChangePublicInformationForm!: boolean;

  public changeEmailForm!: FormGroup;
  public changeUsernameForm!: FormGroup;
  public changePasswordForm!: FormGroup;
  public changeProfileImageForm!: FormGroup;
  public deleteProfileForm!: FormGroup;
  public changePublicInformationForm!: FormGroup;

  private enterPassword!: FormControl;
  private enterEmail!: FormControl;
  private enterUsername!: FormControl;
  private enterNewPassword!: FormControl;
  private enterNewPasswordRepeat!: FormControl;
  private enterNewImage!: FormControl;
  private enterDesignation!: FormControl;
  private enterPreferredLanguage!: FormControl;
  private enterfunFact!: FormControl;

  public submitted!: boolean;
  public successText!: string;
  public errorText!: string;
  public editProfileBtnText!: string;
  public fileName!: string;
  public requestInProgress!: boolean;

  public userName!: string;
  public email!: string;
  public profileImage!: string;
  public designation!: string;
  public subtitleUpload?: number;
  public subtitleDownload?: number;
  public funFact?: string;
  public prefferedLanguage?: string;
  public registrationDate?: Date;
  public userAdminStatus!: boolean;
  public showedUserAdminStatus!: boolean;

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

  public get showChangePublicInformationForm(): boolean {
    return this._showChangePublicInformationForm;
  }


  constructor(private formBuilder: FormBuilder, public userService: UserService, private renderer: Renderer2,
    private tokenValidator: TokenValidatorService, public translate: TranslateService, private languageService: LanguageService,
    private userLanguageService: UserlanguageService, private route: ActivatedRoute, private router: Router,
    public followerService: FollowerServiceService) {
  }

  async ngOnInit(): Promise<void> {
    this.userService.addFriendComponentClicked = false;
    this.userService.userSearchComponentClicked = false;
    this.route.params.subscribe(async (params: Params) => {
      if (!isNaN(params['id'])) {
        this.currentID = parseInt(params['id']);
        this.userService.currentUserParamsID = parseInt(params['id']);
        this.userService.getUserByID(this.currentID).subscribe({
          next: ((response) => {
            this.userService.userData = response.body;
            this.userService.userData.profileImage = environment.baseAPIURL + '/' + this.userService.userData.profileImage;
            this.userLanguageService.getUserLanguage(this.userService.userData.username).subscribe((data) => this.userService.userData.prefferedLanguages = data);
            this.shouldLoad = true;
            this.showedUserAdminStatus=this.userService.userData.isAdmin
          }),
          error: ((err) => { this.router.navigate(['**']); })
        })
      }
      else {
        this.router.navigate(['**']);
      }

    })

    let selectedUser = await this.userService.getCurrentUser();
    if (selectedUser?.id == this.userService.currentUserParamsID) {
      this.userService.isButtonHidden = false;
    } else {
      this.userService.isButtonHidden = true;
    }

    this._showEditProfile = false;
    this._showChangeEmailForm = true;
    this._showChangeUsernameForm = false;
    this._showChangePasswordForm = false;
    this._showChangeProfileImageForm = false;
    this._showDeleteProfileForm = false;
    this._showChangePublicInformationForm = false;
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
    this.enterDesignation = new FormControl('', [Validators.maxLength(25)]);
    this.enterPreferredLanguage = new FormControl();
    this.enterfunFact = new FormControl('', [Validators.maxLength(25)]);

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

    this.changePublicInformationForm = this.formBuilder.group({
      enterPassword: this.enterPassword,
      enterDesignation: this.enterDesignation,
      enterfunFact: this.enterfunFact,
      enterPreferredLanguage: this.enterPreferredLanguage
    });

    this.languageService.getAllLanguages().subscribe((result: Language[]) => {
      this.getLanguages = result;
    });
    this.adminCheck()
  }

  async ngAfterViewInit() {
    this.loggedUser = await this.userService.getCurrentUser();


    if (!this.tokenValidator.validateToken()) {
      this.loggedUser = null;
    }

    if (this.currentID != null || this.currentID != undefined) {
      this.followCount = await this.followerService.GetFollowersOfUser(this.currentID);
      this.shouldLoadFollowCount = true;
    }
    // console.log(this.followCount.getfollowerCount);
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

  async togglePublicInfo(): Promise<void> {

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
      this._showChangePublicInformationForm = false;
    }
    else if (input.id == "changeUsernameBtn") {
      this._showChangeUsernameForm = !this._showChangeUsernameForm;
      this._showChangePasswordForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showDeleteProfileForm = false;
      this._showChangePublicInformationForm = false;
    }
    else if (input.id == "changePasswordBtn") {
      this._showChangePasswordForm = !this._showChangePasswordForm;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showDeleteProfileForm = false;
      this._showChangePublicInformationForm = false;
    }
    else if (input.id == "changeProfileImageBtn") {
      this._showChangeProfileImageForm = !this._showChangeProfileImageForm;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangePasswordForm = false;
      this._showDeleteProfileForm = false;
      this._showChangePublicInformationForm = false;
    }
    else if (input.id == "deleteProfileBtn") {
      this._showDeleteProfileForm = true;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showChangePasswordForm = false;
      this._showChangePublicInformationForm = false;
    } else if (input.id == 'editPublicInformationBtn') {
      this._showDeleteProfileForm = false;
      this._showChangeUsernameForm = false;
      this._showChangeEmailForm = false;
      this._showChangeProfileImageForm = false;
      this._showChangePasswordForm = false;
      this._showChangePublicInformationForm = true;
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
      error: async (err) => {
        var result = await firstValueFrom(this.translate.get('EmailChangeError'));
        this.successText = "";
        this.errorText = result;
        console.error(err.error);
      },
      complete: async () => {
        var result = await firstValueFrom(this.translate.get('EmailChangeSuccess'));
        this.successText = result;
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
      error: async (err) => {
        var result = await firstValueFrom(this.translate.get('UsernameChangeError'));
        this.successText = "";
        this.errorText = result;
        console.error(err.error);
      },
      complete: async () => {
        var result = await firstValueFrom(this.translate.get('UsernameChangeSuccess'));
        this.successText = result;
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
      error: async (err) => {
        var result = await firstValueFrom(this.translate.get('PasswordChangeError'));
        this.successText = "";
        this.errorText = result;
        console.error(err.error);
      },
      complete: async () => {
        var result = await firstValueFrom(this.translate.get('PasswordChangeSuccess'));
        this.successText = result;
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
      error: async (err) => {
        var result = await firstValueFrom(this.translate.get('ProfileImageChangeError'));
        this.successText = "";
        this.errorText = result;
        console.error(err.error);
      },
      complete: async () => {
        var result = await firstValueFrom(this.translate.get('ProfileImageChangeSuccess'));
        this.successText = result;
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
      error: async (err) => {
        var result = await firstValueFrom(this.translate.get('DeleteAccountError'));
        this.successText = "";
        this.errorText = result;
        console.error(err.error);
      },
      complete: async () => {
        var result = await firstValueFrom(this.translate.get('DeleteAccountSuccess'));
        this.successText = result;
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  private reloadPage(): void {
    setTimeout(() => {
      window.location.reload();
    }, 1500);
  }

  changePublicInformation(): void {
    this.submitted = true;

    if (this.changePublicInformationForm.invalid) {
      return;
    }

    this.requestInProgress = true;

    var pubInfoChange: UserUpdate = {
      password: this.enterPassword.value,
      designation: this.enterDesignation.value,
      funFact: this.enterfunFact.value
    }

    this.userService.putUser(pubInfoChange, this.loggedUser!.username).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed public information";
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });

    if (this.prefferedLanguage != null) {
      this.userLanguageService.postUserLanguage(this.userService.userData.username, this.prefferedLanguage!).subscribe({
        next: (response) => console.log(response),
        error: async (err) => {
          this.successText = "";
          this.errorText = err.error;
          console.error(err.error);
        },
        complete: () => {
          this.successText = "Succesfully changed public information";
          this.errorText = "";
          this.requestInProgress = false;
          this.reloadPage();
        }
      });
    }
  }

  deleteUserLanguage() {
    this.userLanguageService.deleteUserLanguage(this.loggedUser!.username, this.prefferedLanguage!).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        this.successText = "";
        this.errorText = err.error;
        console.error(err.error);
      },
      complete: () => {
        this.successText = "Succesfully changed public information";
        this.errorText = "";
        this.requestInProgress = false;
        this.reloadPage();
      }
    });
  }

  async banUser() {
    console.log(`${this.loggedUser} - ${this.userService.userData.id}`);
    await this.userService.banUser(this.loggedUser, this.userService.userData.id).subscribe({
      next: (response) => { console.log(response); },
      error: (err) => { console.log(`Ban method failed!`); console.log(err); },
      complete: () => { console.log('Ban method ran successfully!'); window.location.reload(); }
    });
  }
  async makeAdmin() {
    await this.userService.makeAdmin(this.loggedUser, this.userService.userData.id).subscribe({
      next: (response) => { console.log(response); },
      error: (err) => { console.log(`Making an admin failed!`); console.log(err); },
      complete: () => { console.log('Making an admin ran successfully!'); }
    });
    window.location.reload();
  }
  async removeAdmin() {
    await this.userService.removeAdmin(this.loggedUser, this.userService.userData.id).subscribe({
      next: (response) => { console.log(response); },
      error: (err) => { console.log(`Removing an admin failed!`); console.log(err); },
      complete: () => { console.log('Removing an admin ran successfully!'); }
    });
    window.location.reload();
  }
  gotoMessages() {
    this.router.navigate([`Profile/${this.userService.userData.id}/directMessages`]);
    this.userService.addFriendComponentClicked = false;
    this.userService.userSearchComponentClicked = false;
  }
  gotoAddFriends() {
    this.userService.addFriendComponentClicked = !this.userService.addFriendComponentClicked;
    this.userService.userSearchComponentClicked = false;
    this.router.navigate([`addFriends`], { relativeTo: this.route });
  }
  gotoSearchUserComponent() {
    this.userService.userSearchComponentClicked = !this.userService.userSearchComponentClicked;
    this.router.navigate([`searchUser`], { relativeTo: this.route });
  }
  async adminCheck(){
    var userCurrent =await this.userService.getCurrentUser()
    this.userAdminStatus=userCurrent!.isAdmin
    if(this.showedUserAdminStatus)
      this.designation="Admin"
    else
      this.designation="Member"
  }
}