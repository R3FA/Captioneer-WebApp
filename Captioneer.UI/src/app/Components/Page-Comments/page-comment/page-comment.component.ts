import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommentViewModel } from 'src/app/models/comment-viewmodel';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { CommentService } from 'src/app/services/commentservice.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-page-comment',
  templateUrl: './page-comment.component.html',
  styleUrls: ['./page-comment.component.css']
})
export class PageCommentComponent implements OnInit {

  @ViewChild("commentInput") private commentInput! : ElementRef<HTMLTextAreaElement>

  isAdmin: boolean = true;
  getMovieObject = localStorage.getItem('selected movie');
  asObjectMovie = JSON.parse(this.getMovieObject!);

  shouldLoad : boolean = false;
  subtitle! : any;
  comments! : CommentViewModel[] | null;
  commentValid! : boolean;
  loggedUser! : UserViewModel | null;

  totalPageCount! : number;
  pageIndex : number = 1;

  constructor(private commentService : CommentService, private userService : UserService) { }
  
  async ngOnInit(): Promise<void> {

    this.commentValid = true; 
    this.loggedUser = await this.userService.getCurrentUser();

    this.subtitle = JSON.parse(<string>window.sessionStorage.getItem("HomeSubtitleDownloads"))[0];

    await this.changePage(1);

    this.shouldLoad = true;
  }

  async changePage(page : number) : Promise<void> {
    this.pageIndex = page;
    
    if (window.localStorage.getItem("isTVShow") == "true")  {
      this.comments = await this.commentService.getSubtitleTvShowComments(this.subtitle.subMovieID, this.pageIndex);
    }
    else {
      this.comments = await this.commentService.getSubtitleMovieComments(this.subtitle.subMovieID, this.pageIndex);
    }

    if (this.comments != undefined && this.comments != null && this.comments.length != 0) {
      for (var i = 0; i < this.comments?.length; i++)
      {
        var profileImage = await this.userService.getUserProfileImage(this.comments[i].username);
        this.comments[i].image = profileImage != null ? `${environment.baseAPIURL}/${profileImage}` : "assets/Pictures/userIcon.png";
      }

      this.totalPageCount = this.comments[0].totalPages!;
    }
    else {
      this.pageIndex = 0;
      this.totalPageCount = 0;
    }
  }

  async postComment() : Promise<void> {

    if (this.commentInput.nativeElement.value == "" || this.loggedUser == null) {
      this.commentValid = false;
    }
    else {
      this.commentValid = true;

      if (!this.loggedUser) {
        window.location.reload();
        return;
      }

      var newComment : CommentViewModel = {
        username: this.loggedUser.username,
        content: this.commentInput.nativeElement.value
      } 

      window.localStorage.getItem("isTVShow") == "true"? newComment.subtitleTvShowId = this.subtitle.subMovieID : newComment.subtitleMovieId = this.subtitle.subMovieID;
      this.commentService.postComment(newComment);
    }
  }

  async getImage(username : string) : Promise<string | null> {

    return new Promise((resolved) => {
      resolved(`${environment.baseAPIURL}\\images\\users\\randomUser227032023163907.jpg`);
    })
  }
}
