import { Component, OnInit } from '@angular/core';
import { MovieViewModel } from 'src/app/models/movie-viewmodel';

@Component({
  selector: 'app-page-comment',
  templateUrl: './page-comment.component.html',
  styleUrls: ['./page-comment.component.css']
})
export class PageCommentComponent implements OnInit {

  constructor() { }
  isAdmin: boolean = true;
  getMovieObject = localStorage.getItem('selected movie');
  asObjectMovie = JSON.parse(this.getMovieObject!);

  getSubtitleObject = sessionStorage.getItem('SubtitleDownloads');
  asObjectSubtitle = JSON.parse(this.getSubtitleObject!);
  ngOnInit(): void {
  }
}
