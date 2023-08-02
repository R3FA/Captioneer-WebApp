import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserResponse } from 'src/app/models/userresponse-viewmodel';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css']
})
export class UserSearchComponent implements OnInit {

  constructor(private userService: UserService, private router: Router) { }

  public searchedUser!: UserResponse | null;
  public paginationNumbers: number[] = [];

  async ngOnInit(): Promise<void> {
    await this.userService.getUser(1).subscribe({
      next: (response) => {
        this.searchedUser = response.body;
        for (let i = 1; i <= this.searchedUser!.pages; i++) {
          this.paginationNumbers.push(i);
        }
      },
      error: (err) => { console.log(err); },
    })
  }

  async searchingUsers(pageNumber: number) {
    await this.userService.getUser(pageNumber).subscribe({
      next: (response) => {
        this.searchedUser = response.body;
      },
      error: (err) => { console.log(err); },
    })
  }

  async gotoUsersProfile(userLocation: number) {
    window.location.href = (`Profile/${this.searchedUser?.users?.at(userLocation)?.id}`);
    this.userService.userSearchComponentClicked = false;
  }
}