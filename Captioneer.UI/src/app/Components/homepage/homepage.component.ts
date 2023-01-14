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
  constructor(private http:HttpClient,private router:Router,private page:PaginationService) { }
  ngOnInit():void{ 
    this.isTVShow=false;
    this.getData();
  }
  getData(){
    this.page.getMovieData().subscribe(
      (data)=>{
        this.Movies=data;
        this.selected=this.Movies[0];
      }
    )
    this.page.getTVShowData().subscribe(
      (data)=>{
        this.TVShows=data;
      }
    )
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
