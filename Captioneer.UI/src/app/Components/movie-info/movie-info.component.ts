import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { ActorMovies } from 'src/app/models/actor-movies';
import { empty, firstValueFrom, Observable } from 'rxjs';
import { ThisReceiver } from '@angular/compiler';
import { MovieViewModel } from 'src/app/models/movie-viewmodel';
import { FavoriteMoviesService } from 'src/app/services/favoritemovies.service';
import { UserService } from 'src/app/services/user.service';
import { Loader } from '@googlemaps/js-api-loader';
import { TokenValidatorService } from 'src/app/services/token-validator.service'
import {Language} from 'src/app/models/language'
import{SubtitleDownloads} from 'src/app/models/subtitle-downloads'
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-movie-info',
  templateUrl: './movie-info.component.html',
  styleUrls: ['./movie-info.component.css']
})
export class MovieInfoComponent implements OnInit {

  constructor(private httpClient: HttpClient, private userService: UserService, private favoriteMovieService: FavoriteMoviesService, private tokenValidation: TokenValidatorService) { }
  movie: any;
  movieObject: any;
  actors!: any;
  mapPlaced!: google.maps.Map
  private url: string = environment.apiURL + "/ActorMovies";
  public favorited!: boolean;
  public displayFavorite!: boolean;
  isAdmin: boolean = false;
  isTVSeries: boolean = false;
  clicked: boolean = false;
  translateClicked: boolean = false;
  selectedLanguage!:any;
  languages!:Language[];
  subtitleDownload!:SubtitleDownloads[];
  selectedFile!:any;
  public translate!: TranslateService

  async ngOnInit(): Promise<void> {
    let loader = new Loader({
      apiKey: 'AIzaSyCF4XwMfvYr_nME5E_nBbl9WgNqzfk6dLM'
    })
    loader.load().then(() => {
      this.mapPlaced = new google.maps.Map(document.getElementById("map")!, {
        center: { lat: 34.098907, lng: -118.327759 },
        zoom: 6,
      })
      let marker = new google.maps.Marker({
        position: { lat: 34.098907, lng: -118.327759 },
        map: this.mapPlaced,
        draggable: true,
      })

      this.mapPlaced.addListener("click", (mapsMouseEvent: any) => {
        marker.setPosition(mapsMouseEvent.latLng);
      })
      marker.addListener("click", (mapsMouseEvent: any) => {
        marker.setPosition();
      })
    })
    this.isTVSeries=JSON.parse(window.localStorage.getItem('isTVShow')!)
    let newObject = window.localStorage.getItem("selected movie");
    this.movie = newObject;
    this.movieObject = JSON.parse(this.movie);
    this.favorited = false;
    this.displayFavorite = this.userService.getCurrentUser() != null;
    if(!this.isTVSeries)
    {
    this.getMovies().subscribe((data) => {
      this.actors = data;
      localStorage.setItem('actors', JSON.stringify(data));
    })}
    else{
      this.getTVShow().subscribe((data) => {
        this.actors = data;
        localStorage.setItem('actors', JSON.stringify(data));
      })
    }
    this.actors = JSON.parse(localStorage.getItem('actors')!)
    console.log(this.actors)

    var currentUser = await this.userService.getCurrentUser();

    if (currentUser) {

      var favoriteMovies = await firstValueFrom(this.favoriteMovieService.getFavoriteMovies(currentUser!.username))

      if (favoriteMovies.body) {

        favoriteMovies.body.forEach(favoriteMovie => {
          if (favoriteMovie.title == this.movieObject.title && favoriteMovie.year == this.movieObject.year) {
            this.favorited = true;
          }
        });
      }
    }

    this.getLanguages();
    console.log(this.languages)
  }
  getMovies(): Observable<ActorMovies> {
    return this.httpClient.get<ActorMovies>(this.url + '/' + this.movieObject.id);
  }
  getTVShow(): Observable<ActorMovies> {
    return this.httpClient.get<ActorMovies>(environment.apiURL + "/ActorTVShows/" + this.movieObject.id);
  }

  getLanguages(){
    this.httpClient.get<Language>(environment.apiURL + "/Languages").subscribe((data)=>{
    window.sessionStorage.setItem('Language',JSON.stringify(data))
    });
    this.languages=JSON.parse(window.sessionStorage.getItem('Language')!)
  }
  back() {
    localStorage.clear();
    window.location.href = "../home";
  }

  async favoriteMovie(): Promise<void> {

    if (this.userService.getCurrentUser() == null) {
      return;
    }

    if (!this.tokenValidation.validateToken())
      return;


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

    var currentUser = await this.userService.getCurrentUser();

    if (!this.favorited) {
      this.favoriteMovieService.postFavoriteMovie(currentUser!.username, movieModel).subscribe({
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
    else {
      this.favoriteMovieService.deleteFavoriteMovie(currentUser!.username, movieModel).subscribe({
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

  // Faris Skopak TS Code

  boxChecked() {
    if (this.clicked === false) {
      this.clicked = true;
      console.log('upaljeno');
    }
    else {
      this.clicked = false;
      console.log('ugaseno');
    }
  }

  translateCheck() {
    if (!this.translateClicked)
      this.translateClicked = true;
    else {
      this.translateClicked = false;
    }
  }
  setLanguage(value:any){
    this.selectedLanguage=value
  }
  getUploaders(){
    console.log(this.selectedLanguage)
    this.httpClient.get<SubtitleDownloads>(environment.apiURL + "/OpenSubtitles"+"/"+this.movieObject.imdbId+"/"+this.selectedLanguage).subscribe((data)=>{
      window.sessionStorage.setItem('SubtitleDownloads',JSON.stringify(data))
      });
      this.subtitleDownload=JSON.parse(window.sessionStorage.getItem('SubtitleDownloads')!)
      window.sessionStorage.removeItem('SubtitleDownloads')
  }
  loadFileId(value:any){
    this.selectedFile=value;
    console.log(this.selectedFile)
  }
  downloadSubtitle(){
    this.httpClient.post<string>(environment.apiURL + "/OpenSubtitles"+"/"+this.selectedFile,this.selectedFile).subscribe((data)=>{
      var link!:any;
      link=data;
      window.open(link.link)
      });
  }
}
