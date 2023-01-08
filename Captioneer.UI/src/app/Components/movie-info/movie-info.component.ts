import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { ActorMovies } from 'src/app/models/actor-movies';
import { Observable } from 'rxjs';
import { ThisReceiver } from '@angular/compiler';
import { MovieViewModel } from 'src/app/models/movie-viewmodel';
import { FavoriteMoviesService } from 'src/app/services/favoritemovies.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-movie-info',
  templateUrl: './movie-info.component.html',
  styleUrls: ['./movie-info.component.css']
})
export class MovieInfoComponent implements OnInit {

  constructor(private httpClient: HttpClient, private userService : UserService, private favoriteMovieService : FavoriteMoviesService) { }
  movie:any;
  movieObject:any;
  actors!:any;
  private url: string = environment.apiURL + "/ActorMovies";
  public favorited! : boolean;
  public displayFavorite! : boolean;

  ngOnInit(): void {
    let newObject = window.localStorage.getItem("selected movie");
    this.movie=newObject;
    this.movieObject=JSON.parse(this.movie);
    this.favorited = false;
    this.displayFavorite = this.userService.getCurrentUser() != null;
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

  favoriteMovie() : void {

    if (this.userService.getCurrentUser() == null) {
      return;
    }

    let movieModel = new MovieViewModel();
    movieModel.title = this.movieObject.title;
    movieModel.imdbId = this.movieObject.imdbId;
    movieModel.synopsis = this.movieObject.synopsis;
    movieModel.year = this.movieObject.year;
    movieModel.runtime = this.movieObject.runtime;
    movieModel.imdbRatingValue = this.movieObject.imdbRatingValue;
    movieModel.imdbRatingCount = this.movieObject.imdbRatingCount;
    movieModel.rottenTomatoesValue = this.movieObject.rottenTomatoesValue;
    movieModel.metacriticValue = this.movieObject.metacriticValue;
    movieModel.coverArt = this.movieObject.coverArt;

    if (!this.favorited) {
      this.favoriteMovieService.postFavoriteMovie(this.userService.getCurrentUser()!.username, movieModel).subscribe({
        next: (response) => {
        },
        error: (err) => {
          console.error(err);
        },
        complete: () => {
          this.favorited = true;
        }
      });
    }
    else  {
      this.favoriteMovieService.deleteFavoriteMovie(this.userService.getCurrentUser()!.username, movieModel).subscribe({
        next: (response) => {
        },
        error: (err) => {
          console.error(err);
        },
        complete: () => {
          this.favorited = false;
        }
      });
    }
  }
}
