import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
import { MovieViewModel } from '../../../models/movie-viewmodel'
import { MovieService } from 'src/app/services/movie.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header-component',
  templateUrl: './header-component.component.html',
  styleUrls: ['./header-component.component.css']
})
export class HeaderComponentComponent implements OnInit {

  movies: MovieViewModel[] = [];
  movieName: string = "";
  myControl = new FormControl('');
  finalData!: Observable<MovieViewModel[]>;
  isNotLogedIn!:boolean;
  name!:string;
  user!:UserViewModel;

  onClick() {
    this.moviesService.getMovieByParameter(this.movieName).subscribe({
      next: (response) => console.log(response),
      error: (err) => {
        console.error(err.error);
      },
      complete: () => {
        console.log("RADI");
      }
    });
    window.location.reload();
  }
  constructor(private moviesService: MovieService,private http:HttpClient,private router:Router,private userService:UserService, public translate : TranslateService) {
    this.moviesService.getMovies().subscribe((result: MovieViewModel[]) => (this.movies = result));
  }

  ngOnInit(): void {
    this.finalData = this.myControl.valueChanges.pipe(startWith(''), map(item => {
      const name = item;
      return name ? this._filter(name as string) : this.movies;
    }));
    if(sessionStorage.getItem("email")==null)
    {
      this.isNotLogedIn=true;
    }
    else
    {
      var temp=sessionStorage.getItem("email");
      this.getData(temp)
      console.log("okay");
    }

  }

  private _filter(moviesName: string): MovieViewModel[] {
    const filterValue = moviesName.toLowerCase();
    console.log(filterValue);
    return this.movies.filter(opt => opt.title.toLowerCase().includes(filterValue));
  }

  selectMovie(movieName: any) {
    alert(movieName);
  }
  getData(email:any){
    this.userService.getUserByEmail(email).subscribe(
      (data)=>{
        var userName=data.body?.username;
        this.name=userName!;
      }
    )
  }
  signOut()
  {
    
    sessionStorage.clear();
    window.location.href = "../signin";
  }

  public getLangName(lang : string) : string {

    switch(lang)
    {
      case "en": return "English";
      case "bs": return "Bosanski";
      case "de": return "Deutsche";
    }

    return "Unknown";
  }

  public switchLang(lang : string) : void {

    this.translate.use(lang);
  }
}
