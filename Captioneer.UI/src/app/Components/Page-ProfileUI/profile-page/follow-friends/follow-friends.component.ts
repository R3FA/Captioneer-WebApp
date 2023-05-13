import { Component, OnInit } from '@angular/core';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { FollowerServiceService } from 'src/app/services/follower.service.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-follow-friends',
  templateUrl: './follow-friends.component.html',
  styleUrls: ['./follow-friends.component.css']
})
export class FollowFriendsComponent implements OnInit {

  constructor(private followerService: FollowerServiceService, private userService: UserService) { }

  public AddfriendsUsername: string = "";
  public DeletefriendsUsername: string = "";
  ngOnInit(): void {
  }

  private reloadPage(): void {
    setTimeout(() => {
      window.location.reload();
    }, 1500);
  }


  addFriend() {
    this.followerService.PostFollower(this.userService.userData, this.AddfriendsUsername).subscribe({
      next: (response) => { console.log(response); },
      error: (err) => { console.log(err) },
      complete: () => { this.reloadPage(); }
    });
  }

  deleteFriend() {
    this.followerService.DeleteFollower(this.userService.userData, this.DeletefriendsUsername).subscribe({
      next: (response) => { console.log(response); },
      error: (err) => { console.log(err) },
      complete: () => { this.reloadPage(); }
    })
  }
}