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
  coverart!:string;
  temp:any;
  selected:any;
  constructor(private http:HttpClient,private router:Router,private page:PaginationService) { }
  ngOnInit():void{ 
    this.getData();
  }
  getData(){
    this.page.getData().subscribe(
      (data)=>{
        this.Movies=data;
        console.log(this.Movies)
        this.selected=this.Movies[0];
      }
    )
  }
  loadMovie(movie:any){
    this.selected=movie;
    console.log(this.selected)
  }
}
