import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { PaginationService } from 'src/app/services/pagination.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
  // Movies:any[]=[];
  Movies:any=[];
  TVShows:any=[];
  coverart!:string;
  temp:any;
  selected:any;
  isTVShow!:boolean;
  _page = 1; // Current page
  pageSize = 10; // Number of records per page
  totalRecords = 0; // Total number of records
  totalPages = 0; // Total number of pages  
  _pageTV = 1; // Current page
  pageSizeTV = 10; // Number of records per page
  totalRecordsTV = 0; // Total number of records
  totalPagesTV = 0; // Total number of pages
  constructor(private http:HttpClient,private router:Router,private page:PaginationService) { }
  ngOnInit():void{ 
    this.isTVShow=false;
    this.getData();
  }
  getData(){
    this.page.getMovieData(this._page).subscribe(
      (result)=>{
        this.Movies=result;
        this.selected=this.Movies.data[0];
        this.totalRecords = this.Movies.totalRecords;
        this.totalPages = this.Movies.totalPages;
      }
    )
    this.page.getTVShowData(this._pageTV).subscribe(
      (result)=>{
        this.TVShows=result;
        this.selected=this.TVShows.data[0];
        this.totalRecordsTV = this.TVShows.totalRecords;
        this.totalPagesTV = this.TVShows.totalPages;
      }
    )
  }
  previousPage() {
    if (this._page > 1) {
      this._page--;
      this.getData();
    }
  }

  nextPage() {
    if (this._page < this.totalPages) {
      this._page++;
      this.getData();
    }
  }
  previousPageTV() {
    if (this._pageTV > 1) {
      this._pageTV--;
      this.getData();
    }
  }

  nextPageTV() {
    if (this._pageTV < this.totalPagesTV) {
      this._pageTV++;
      this.getData();
    }
  }
  setMovies(){
    this.isTVShow=false;
    console.log(this.isTVShow)
  }
  setTVShow(){
    this.isTVShow=true;
    console.log(this.isTVShow)
  }
  loadMovie(movie:any){
    this.selected=movie;
  }
  saveMovie(){
    localStorage.setItem('selected movie',JSON.stringify(this.selected));
    localStorage.setItem('isTVShow',JSON.stringify(this.isTVShow));
    window.location.href = "../movie"
  }
}
