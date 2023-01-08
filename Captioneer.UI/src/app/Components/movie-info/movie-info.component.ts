import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { ActorMovies } from 'src/app/models/actor-movies';
import { Observable } from 'rxjs';
import { ThisReceiver } from '@angular/compiler';
import { Loader } from '@googlemaps/js-api-loader';

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
  mapPlaced!:google.maps.Map
  private url: string = environment.apiURL + "/ActorMovies";

  ngOnInit(): void {
    let loader=new Loader({
      apiKey:'AIzaSyCF4XwMfvYr_nME5E_nBbl9WgNqzfk6dLM'
    })
    loader.load().then(()=>{
      this.mapPlaced=new google.maps.Map(document.getElementById("map")!,{
        center:{lat: 34.098907, lng: -118.327759},
        zoom: 6,
      })
      let marker= new google.maps.Marker({
        position:{lat: 34.098907, lng: -118.327759},
        map:this.mapPlaced,
        draggable:true,
      })

      this.mapPlaced.addListener("click", (mapsMouseEvent:any) => {
        // Create a new InfoWindow.
          marker.setPosition(mapsMouseEvent.latLng);
      })
      marker.addListener("click", (mapsMouseEvent:any) => {
        marker.setPosition();
      })
      
    })
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
