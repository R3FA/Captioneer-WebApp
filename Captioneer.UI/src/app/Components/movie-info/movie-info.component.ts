import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { ActorMovies } from 'src/app/models/actor-movies';
import { Observable } from 'rxjs';
import { ThisReceiver } from '@angular/compiler';

@Component({
  selector: 'app-movie-info',
  templateUrl: './movie-info.component.html',
  styleUrls: ['./movie-info.component.css']
})
export class MovieInfoComponent implements OnInit {

  constructor(private httpClient: HttpClient) { }
  movie:any;
  movieObject:any;
  actors!:any;
  private url: string = environment.apiURL + "/ActorMovies";

  ngOnInit(): void {
    let newObject = window.localStorage.getItem("selected movie");
    this.movie=newObject;
    this.movieObject=JSON.parse(this.movie);
    this.getMovies().subscribe( (data)=>{
      this.actors=data;
      localStorage.setItem('actors',JSON.stringify(data));
    })
    this.actors=JSON.parse(localStorage.getItem('actors')!)
    console.log(this.actors)
  }
  getMovies(): Observable<ActorMovies> {
    return this.httpClient.get<ActorMovies>(this.url+'/'+this.movieObject.id);
  }
  back()
  {
    localStorage.clear();
    window.location.href = "../home";
  }
}
