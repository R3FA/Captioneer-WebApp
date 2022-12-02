import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
import { Movies } from 'src/app/models/movies';
import { MovieService } from 'src/app/services/movie.service';

@Component({
  selector: 'app-header-component',
  templateUrl: './header-component.component.html',
  styleUrls: ['./header-component.component.css']
})
export class HeaderComponentComponent implements OnInit {

  movies: Movies[] = [];
  movieName: string = "";
  myControl = new FormControl('');
  finalData!: Observable<Movies[]>;

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
  constructor(private moviesService: MovieService) {
    this.moviesService.getMovies().subscribe((result: Movies[]) => (this.movies = result));
  }

  ngOnInit(): void {
    this.finalData = this.myControl.valueChanges.pipe(startWith(''), map(item => {
      const name = item;
      return name ? this._filter(name as string) : this.movies;
    }))
  }

  private _filter(moviesName: string): Movies[] {
    const filterValue = moviesName.toLowerCase();
    console.log(filterValue);
    return this.movies.filter(opt => opt.title.toLowerCase().includes(filterValue));
  }

  selectMovie(movieName: any) {
    alert(movieName);
  }
}
