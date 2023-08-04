import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { UserResponse } from 'src/app/models/userresponse-viewmodel';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css']
})
export class UserSearchComponent implements OnInit {

  constructor(private userService: UserService, private router: Router) { }

  public usersList!: UserResponse | null;
  public searchedUser!: UserViewModel | null;
  public paginationNumbers: number[] = [];
  public stringValue: string = "";
  public isOneUser: boolean = false;

  async ngOnInit(): Promise<void> {
    this.isOneUser = false;
    await this.userService.getUser(1).subscribe({
      next: (response) => {
        this.usersList = response.body;
        for (let i = 1; i <= this.usersList!.pages; i++) {
          this.paginationNumbers.push(i);
        }
      },
      error: (err) => { console.log(err); },
    })
  }

  async searchingUsers(pageNumber: number) {
    this.stringValue = "";
    this.searchedUser = null;
    this.isOneUser = false;
    await this.userService.getUser(pageNumber).subscribe({
      next: (response) => {
        this.usersList = response.body;
      },
      error: (err) => { console.log(err); },
    })
  }

  async searchUserByEmailOrUsername(email: string, username: string) {
    this.isOneUser = true;
    await this.userService.getUserByEmail(email, username).subscribe({
      next: (response) => {
        this.searchedUser = response.body;
        console.log(this.searchedUser);
      },
      error: (err) => { this.searchedUser = null; }
    })
  }
  async gotoUsersProfile(userLocation: number) {
    window.location.href = (`Profile/${this.usersList?.users?.at(userLocation)?.id}`);
    this.userService.userSearchComponentClicked = false;
  }

  async gotoUserProfile(userID: number) {
    window.location.href = (`Profile/${userID}`);
    this.userService.userSearchComponentClicked = false;
  }
}