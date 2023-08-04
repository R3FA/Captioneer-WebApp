import { AfterViewInit, Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { ActorMovies } from 'src/app/models/actor-movies';
import { empty, firstValueFrom, Observable } from 'rxjs';
import { MovieViewModel } from 'src/app/models/movie-viewmodel';
import { FavoriteMoviesService } from 'src/app/services/favoritemovies.service';
import { FavoriteTVShowsService } from 'src/app/services/favoritetvshows.service';
import { UserService } from 'src/app/services/user.service';
import { Loader } from '@googlemaps/js-api-loader';
import { TokenValidatorService } from 'src/app/services/token-validator.service'
import {Language} from 'src/app/models/language'
import{SubtitleDownloads} from 'src/app/models/subtitle-downloads'
import { TranslateService } from '@ngx-translate/core';
import { TVShowViewModel } from 'src/app/models/tvshow-viewmodel';
import{LanguageService} from 'src/app/services/language.service'
import { Creator } from 'src/app/models/creator';
import { CreatorService } from 'src/app/services/creator.service';
import { SubtitletranslationService } from 'src/app/services/subtitletranslation.service';
import { TranslationPostModel } from 'src/app/models/translation-post';
import { HomeSubtitleDownlaods } from 'src/app/models/home-subtitle-downlaods';
import { getDatabase } from 'firebase/database';
import { ref, set, push } from 'firebase/database';
import { initializeApp } from 'firebase/app';
import { saveAs } from 'file-saver';


@Component({
  selector: 'app-movie-info',
  templateUrl: './movie-info.component.html',
  styleUrls: ['./movie-info.component.css']
})
export class MovieInfoComponent implements OnInit,AfterViewInit {
  constructor(
    private httpClient: HttpClient,
    private userService: UserService,
    private favoriteMovieService: FavoriteMoviesService,
    private favoriteTVShowsService : FavoriteTVShowsService,
    private tokenValidation: TokenValidatorService,
    private languageService:LanguageService,
    private creatorService:CreatorService,
    private subtitleTranslationService : SubtitletranslationService,)
    { 
    }

  movie: any;
  movieObject: any;
  actors!: any;
  mapPlaced!: google.maps.Map
  private url: string = environment.apiURL + "/ActorMovies";
  public favorited!: boolean;
  public displayFavorite!: boolean;
  isAdmin: boolean = false;
  isTVSeries: boolean = false;
  isInHouse:boolean=false;
  clicked: boolean = false;
  translateClicked: boolean = false;
  selectedLanguage!:any;
  selectedLanguage2!:any;
  languages!:Language[];
  subtitleDownload!:SubtitleDownloads[];
  homeSubtitleDownload!:HomeSubtitleDownlaods[];
  public selectedFile! : SubtitleDownloads;
  public selectedFileInHouse! : HomeSubtitleDownlaods;
  public translate!: TranslateService;
  creators!: Creator[];
  public fileName: string="";
  file!:File;
  validSubtitle:boolean=false;
  fileSize:any;
  type:string="";
  uploadButtonPressed:boolean=false;
  seasonNumber?:number;
  episodeNumber?:number;
  subtitleRating:number=5;
  episodeeNumber!:number;

  public translatableLanguages! : Language[] | null;
  public languageToTranslate! : Language | null;

  async ngAfterViewInit(): Promise<void> {

    this.languages=await this.getLanguages();
    this.translatableLanguages = await this.subtitleTranslationService.getTranslatableLanguages();
    this.creators=await this.getCreators();
  }

  async ngOnInit(): Promise<void> {
    const firebaseConfig = {
      apiKey: "AIzaSyAzI3RbycvBr3uCCcd6WnLV6iiFeU9EOYI",
      authDomain: "captioneer-4c392.firebaseapp.com",
      databaseURL: "https://captioneer-4c392-default-rtdb.firebaseio.com",
      projectId: "captioneer-4c392",
      storageBucket: "captioneer-4c392.appspot.com",
      messagingSenderId: "803329760083",
      appId: "1:803329760083:web:acd5573927b6e8da091fa6",
      measurementId: "G-EMPML47RVX"
    };
    const app = initializeApp(firebaseConfig);
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

      if (this.isTVSeries)  {

        var favoriteTVShows = await this.favoriteTVShowsService.getFavoriteShows(currentUser.username);

        if (favoriteTVShows) {

          favoriteTVShows.forEach(favoriteTVShow => {
            if (favoriteTVShow.title == this.movieObject.title && favoriteTVShow.year == this.movieObject.year) {
              this.favorited = true;
            }
          });
        }
      }
      else  {        

        var favoriteMovies = await this.favoriteMovieService.getFavoriteMovies(currentUser.username);
  
        if (favoriteMovies) {
  
          favoriteMovies.forEach(favoriteMovie => {
            if (favoriteMovie.title == this.movieObject.title && favoriteMovie.year == this.movieObject.year) {
              this.favorited = true;
            }
          });
        }
      }

  }
  
  }
  getMovies(): Observable<ActorMovies> {
    return this.httpClient.get<ActorMovies>(this.url + '/' + this.movieObject.id);
  }
  getTVShow(): Observable<ActorMovies> {
    return this.httpClient.get<ActorMovies>(environment.apiURL + "/ActorTVShows/" + this.movieObject.id);
  }
  async getLanguages():Promise<Language[]>{
        return new Promise((resolve)=>{
          this.languageService.getAllLanguages().subscribe((data)=>{
            resolve(data);
          })
        })
    }
  
  async getCreators():Promise<Creator[]>{
    return new Promise((resolve)=>{
      this.creatorService.getCreators(this.isTVSeries,this.movieObject.id).subscribe((data)=>{
        resolve(data);
      })
    })
  }

  back() {
    localStorage.clear();
    window.location.href = "";
  }

  async favoriteTVShow() : Promise<void> {

    var currentUser = await this.userService.getCurrentUser();

    if (currentUser == null) {
      return;
    }

    if (!this.tokenValidation.validateToken())
      return;

      let tvShowModel = new TVShowViewModel();
      tvShowModel.title = this.movieObject.title;
      tvShowModel.imdbId = this.movieObject.imdbId;
      tvShowModel.synopsis = this.movieObject.synopsis;
      tvShowModel.year = this.movieObject.year;
      tvShowModel.imdbRatingValue = this.movieObject.imdbRatingValue;
      tvShowModel.imdbRatingCount = this.movieObject.imdbRatingCount;
      tvShowModel.rottenTomatoesValue = this.movieObject.rottenTomatoesValue;
      tvShowModel.metacriticValue = this.movieObject.metacriticValue;
      tvShowModel.coverArt = this.movieObject.coverArt;

      if (!this.favorited)  {
        this.favoriteTVShowsService.post(currentUser.username, tvShowModel).subscribe({
          next: (response) => {

          },
          error: (err) => {
            console.error(err);
          },
          complete: () => {
            this.favorited = true;
          }
        })
      }
      else {
        this.favoriteTVShowsService.delete(currentUser.username, tvShowModel).subscribe({
          next: (response) => {

          },
          error: (err) => {
            console.error(err);
          },
          complete: () => {
            this.favorited = true;
          }
        })
      }
  }

  async favoriteMovie() : Promise<void> {

    var currentUser = await this.userService.getCurrentUser();
    
    if (currentUser == null) {
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

    if (!this.favorited) {
      this.favoriteMovieService.post(currentUser!.username, movieModel).subscribe({
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
      this.favoriteMovieService.delete(currentUser!.username, movieModel).subscribe({
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

  InHouseChecked(){
    if (this.isInHouse === false) {
      this.isInHouse = true;
    }
    else {
      this.isInHouse = false;
    }
  }
    // Faris Skopak TS Code


  setLanguage(value:any){
    this.selectedLanguage=value;
    console.log(this.selectedLanguage);
  }
  setLanguage2(value:any){
    this.selectedLanguage2=value;
    console.log(this.selectedLanguage2);
  }
  getUploaders(){
    if(typeof this.selectedLanguage==="undefined"){
      alert("Because you did not choose a language it defaulted to english");
      this.selectedLanguage="en";
    }
    if(!this.isTVSeries){
    this.httpClient.get<SubtitleDownloads>(environment.apiURL + "/OpenSubtitles"+"/"+this.movieObject.imdbId+"/"+this.selectedLanguage).subscribe((data)=>{
      window.sessionStorage.setItem('SubtitleDownloads',JSON.stringify(data))
      });
      this.subtitleDownload=JSON.parse(window.sessionStorage.getItem('SubtitleDownloads')!)
      window.sessionStorage.removeItem('SubtitleDownloads')
    this.httpClient.get<HomeSubtitleDownlaods>(environment.apiURL + "/SubtitleMovie?movieId="+this.movieObject.id+"&languageCode="+this.selectedLanguage).subscribe((data)=>{
      window.sessionStorage.setItem('HomeSubtitleDownloads',JSON.stringify(data))
        });
        this.homeSubtitleDownload=JSON.parse(window.sessionStorage.getItem('HomeSubtitleDownloads')!)
        window.sessionStorage.removeItem('HomeSubtitleDownloads')
      }
    else{
      if(typeof this.seasonNumber==="undefined"){
        this.seasonNumber=1;
      }
      if(typeof this.episodeNumber==="undefined"){
        this.episodeNumber=1;
      }
      this.httpClient.get<SubtitleDownloads>(environment.apiURL + "/OpenSubtitles"+"/"+this.movieObject.imdbId+"/"+this.selectedLanguage+"?seasonNumber="+this.seasonNumber+"&episodeNumber="+this.episodeNumber).subscribe((data)=>{
        window.sessionStorage.setItem('SubtitleDownloads',JSON.stringify(data))
        });
        this.subtitleDownload=JSON.parse(window.sessionStorage.getItem('SubtitleDownloads')!)
        window.sessionStorage.removeItem('SubtitleDownloads')
      this.httpClient.get<HomeSubtitleDownlaods>(environment.apiURL + "/SubtitleTVShows/"+this.seasonNumber+"/"+this.episodeNumber+"?"+"movieId="+this.movieObject.id+"&languageCode="+this.selectedLanguage).subscribe((data)=>{
        window.sessionStorage.setItem('HomeSubtitleDownloads',JSON.stringify(data))
          });
          this.homeSubtitleDownload=JSON.parse(window.sessionStorage.getItem('HomeSubtitleDownloads')!)
          window.sessionStorage.removeItem('HomeSubtitleDownloads')
    }
  }

  async downloadSubtitle() : Promise<void> {
    if (!this.translateClicked && !this.isInHouse) {
      this.httpClient.post<string>(environment.apiURL + "/OpenSubtitles"+"/"+this.selectedFile.fileId,this.selectedFile.fileId).subscribe((data)=>{
        var link!:any;
        link=data;
        saveAs(link.link, this.selectedFile.release + ".srt");
        });
    }
    if (!this.translateClicked && this.isInHouse) {
      var currentUser = await this.userService.getCurrentUser();
    if(currentUser==null)
      {
        alert("You have to log in to download a In House made subtitle");
        return;
      }
      if(this.selectedFileInHouse==null)
      {
        alert("Please select a subtitle");
        return;
      }
      if(!this.isTVSeries){

      
      this.httpClient.get(environment.apiURL + "/SubtitleMovie/api/download?subMovieID="+this.selectedFileInHouse.subMovieID+"&userEmail="+currentUser.email, { responseType: 'blob' })
      .subscribe(blob => {
        const downloadUrl = window.URL.createObjectURL(blob);
        const extension = '.srt';
        const link = document.createElement('a');
        link.href = downloadUrl;
        var title=this.movieObject.title
        link.download = `${title.split('.')[0]}${extension}`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(downloadUrl);
      });
    }
    else{
      this.httpClient.get(environment.apiURL + "/SubtitleTVShows/api/download?subMovieID="+this.selectedFileInHouse.subMovieID+"&userEmail="+currentUser.email, { responseType: 'blob' })
      .subscribe(blob => {
        const downloadUrl = window.URL.createObjectURL(blob);
        const extension = '.srt';
        const link = document.createElement('a');
        link.href = downloadUrl;
        var title=this.movieObject.title+"S"+this.seasonNumber+"E"+this.episodeNumber;
        link.download = `${title.split('.')[0]}${extension}`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(downloadUrl);
      });
    }
    }

    if(this.translateClicked) {

      var model = new TranslationPostModel();

      if (this.selectedFile != null) {
        model.release = this.selectedFile.release!;
        model.languageFrom = this.selectedFile.language!;
        model.languageTo = this.languageToTranslate!.languageCode!;
        model.fileID = this.selectedFile.fileId!.toString();
      }
      else {
        console.log(`Language to translate: ${this.languageToTranslate?.languageCode}`);
        model.release = this.selectedFileInHouse.release!;
        model.languageFrom = this.selectedLanguage!;
        model.languageTo = this.languageToTranslate!.languageCode!;
      }

      console.log(model);

      await this.subtitleTranslationService.translate(model);
    }
  }

  async UploadSubtitle(){
    this.uploadButtonPressed=true;
    var currentUser = await this.userService.getCurrentUser();
    if(currentUser==null)
      {
        this.validSubtitle=false;
        alert("You have to log in to upload a subtitle");
        return;
      }
    if(!this.validSubtitle)
    {
      alert("You have to upload a subtitle in a valid .srt format");
      return;
    }
    this.selectedLanguage;
    this.movieObject.id;

    var release=(<HTMLInputElement>document.getElementById("release")).value
    var fps=parseInt((<HTMLInputElement>document.getElementById("fps")).value)
    if(!release)
      release="";
    if(isNaN(fps))
      fps=0;
    const formData: FormData = new FormData();

    formData.append('fileName', this.fileName);
    formData.append('file', this.file);

    console.log(formData);
    if(!this.isTVSeries){
      const params=new HttpParams()
                      .set('movieId',this.movieObject.id)
                      .set('languageCode',this.selectedLanguage2)
                      .set('userEmail',currentUser!.email)
                      .set('release',release)
                      .set('frameRate',fps)

      this.httpClient.post<any>(environment.apiURL+"/SubtitleMovie",formData,{params:params}).subscribe((data)=>
      console.log(data)
    );
    }
    else{
      
      if(typeof this.seasonNumber==="undefined"){
        alert("Beacause you did not enter the season number it's defaulted to season 1")
        this.seasonNumber=1;
      }
      if(typeof this.episodeNumber==="undefined"){
        alert("Beacause you did not enter the episode number it's defaulted to episode 1")
        this.episodeNumber=1;
      }
      const params=new HttpParams()
                      .set('tvShowID',this.movieObject.id)
                      .set('seasonNumber',this.seasonNumber)
                      .set('episodeNumber',this.episodeNumber)
                      .set('languageCode',this.selectedLanguage2)
                      .set('frameRate',fps)
                      .set('release',release)
                      .set('userEmail',currentUser!.email)

      this.httpClient.post<any>(environment.apiURL+"/SubtitleTVShows",formData,{params:params}).subscribe((data)=>
      console.log(data)
    );
    }
  }
  validateFileUpload(event: any): void {
      this.file= event.target.files[0];
      this.fileName = this.file.name;
      let lastDot=this.fileName.lastIndexOf('.');
      this.type=this.fileName.slice(lastDot+1).trim();

      if (this.file.size == 0 && this.file.size>300000) {
        return;
      }
      if (this.type != "srt") {
        return;
    }
    else{
    this.fileSize=this.file.size/1000;
    this.fileSize=Math.round(this.fileSize);
    this.validSubtitle=true;
    }
  }

  switchTranslateLanguage(language : Language) : void {
    this.languageToTranslate = language;
  }

  async Rating() : Promise<void>{
    var currentUser = await this.userService.getCurrentUser();
    if(currentUser==null)
      {
        alert("You have to log in to download a In House made subtitle");
        return;
      }
      if(this.selectedFileInHouse==null)
      {
        alert("Please select a subtitle");
        return;
      }
      if(!this.isTVSeries){
        this.httpClient.put(environment.apiURL + "/SubtitleMovie?subMovieID="+this.selectedFileInHouse.subMovieID+"&userEmail="+currentUser.email+"&userRatingValue="+this.subtitleRating,"",).subscribe((data)=>
        console.log(data)
        );
      }
      else{
        this.httpClient.put(environment.apiURL + "/SubtitleTVShows?subMovieID="+this.selectedFileInHouse.subMovieID+"&userEmail="+currentUser.email+"&userRatingValue="+this.subtitleRating,"",).subscribe((data)=>
        console.log(data)
        );
      }
    window.location.reload();
  }
  Report(){
    console.log(this.selectedFileInHouse);
    console.log(this.movieObject.title);
    console.log(this.selectedLanguage);
    const database = getDatabase();
    const dataRef = ref(database, 'subtitles');
    const newRef = push(dataRef);
    if(this.isTVSeries){
      const epNum:number = Number(this.seasonNumber); 
      set(newRef, {
        isTVShow: this.isTVSeries,
        numberEpisode: this.episodeNumber,
        numberSeason: epNum,
        subMovieID: this.selectedFileInHouse.subMovieID,
        uplaoder: this.selectedFileInHouse.uploader,
        movieID:this.movieObject.id,
        movieTitle:this.movieObject.title,
        language:this.selectedLanguage,
        release:this.selectedFileInHouse.release,
        fps:this.selectedFileInHouse.fps,
      });
    }
    else{
      set(newRef, {
        isTVShow: this.isTVSeries,
        subMovieID: this.selectedFileInHouse.subMovieID,
        uplaoder: this.selectedFileInHouse.uploader,
        movieID:this.movieObject.id,
        movieTitle:this.movieObject.title,
        language:this.selectedLanguage,
        release:this.selectedFileInHouse.release,
        fps:this.selectedFileInHouse.fps,
      });
    }

    alert("Thank you for reporting!");
   }
}
