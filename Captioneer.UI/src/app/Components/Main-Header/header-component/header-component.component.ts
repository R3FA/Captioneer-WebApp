import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
import { MovieViewModel } from '../../../models/movie-viewmodel'
import { MovieService } from 'src/app/services/movie.service';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { TranslateService } from '@ngx-translate/core';
import { UserlanguageService } from 'src/app/services/userlanguage.service';
import { UserLanguageModel } from 'src/app/models/userLanguage-viewmodel';
import { environment } from 'src/environments/environment';
import { getDatabase } from 'firebase/database';
import { ref, set, push, onValue, get, DataSnapshot, remove } from 'firebase/database';
import { initializeApp } from 'firebase/app';
import { AdminService } from 'src/app/services/admin.service';
import { TVShowViewModel } from 'src/app/models/tvshow-viewmodel';
import { TvshowService } from 'src/app/services/tvshow.service';

@Component({
  selector: 'app-header-component',
  templateUrl: './header-component.component.html',
  styleUrls: ['./header-component.component.css']
})

export class HeaderComponentComponent implements OnInit {

  movies: MovieViewModel[] = [];
  userLanguages: any[] = [];
  public userLangauges: UserLanguageModel[] = [];
  movieName: string = "";
  myControl = new FormControl('');
  finalData!: Observable<MovieViewModel[]>;
  isNotLogedIn!: boolean;
  name!: string;
  user!: any;
  loggedUser: any;
  currentUser!: any;
  currentUserID!: number;
  selectedUser!: any;
  numberOfEntries!: number;
  showNotifications: boolean = false;
  userAdminStatus: boolean = false;
  entries: any[] = [];
  entriesSubtitleS: any[] = [];

  public searchValue: string = '';

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
  constructor(private moviesService: MovieService, private http: HttpClient, private router: Router,
    private userService: UserService, public translate: TranslateService, public userLanguage: UserlanguageService,
    public userLanguageService: UserlanguageService, public route: ActivatedRoute, public adminServices: AdminService,
    public tvShowService: TvshowService) {
    this.moviesService.getMovies().subscribe((result: MovieViewModel[]) => (this.movies = result));
  }



  async ngOnInit(): Promise<void> {
    // this.selectedUser = await this.userService.getSelectedUserByID(this.currentUser.id);
    // console.log(this.currentUser);

    this.finalData = this.myControl.valueChanges.pipe(startWith(''), map(item => {
      const name = item;
      return name ? this._filter(name as string) : this.movies;
    }));
    if (sessionStorage.getItem("email") == null) {
      this.isNotLogedIn = true;
    }
    else {
      var temp = sessionStorage.getItem("email");
      this.getData(temp);
      console.log("okay");
    }
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
    await this.getNumberOfEntriesComments().then(numEntries => {
      this.numberOfEntries = numEntries;
    }).catch(error => {
      console.error('Error getting number of entries:', error);
    });
    await this.getNumberOfEntriesSubtitles().then(numEntries => {
      this.numberOfEntries += numEntries;
    }).catch(error => {
      console.error('Error getting number of entries:', error);
    });
    var userCurrent = await this.userService.getCurrentUser()
    this.userAdminStatus = userCurrent!.isAdmin
    await this.getEntries();
  }

  async gotoMyProfile() {
    let selectedUser = await this.userService.getCurrentUser();
    window.location.href = (`Profile/${selectedUser?.id}`);
    this.userService.addFriendComponentClicked = false;
  }

  private _filter(moviesName: string): MovieViewModel[] {
    const filterValue = moviesName.toLowerCase();
    console.log(filterValue);
    return this.movies.filter(opt => opt.title.toLowerCase().includes(filterValue));
  }

  selectMovie(movieName: any) {
    alert(movieName);
  }
  getData(email: any) {
    this.userService.getUserByEmail(email).subscribe(
      (data) => {
        var userName = data.body?.username;
        this.name = userName!;
        // console.log(this.userService.userData);
      }
    )
  }

  signOut() {
    sessionStorage.clear();
    localStorage.clear();
    window.location.href = "";
  }

  async getNumberOfEntriesComments(): Promise<number> {
    const database = getDatabase();
    const dbRef = ref(database, 'comments');
    return get(dbRef).then((snapshot: DataSnapshot) => {
      let count = 0;
      snapshot.forEach(() => {
        count++;
      });
      return count;
    });
  }
  async getNumberOfEntriesSubtitles(): Promise<number> {
    const database = getDatabase();
    const dbRef = ref(database, 'subtitles');
    return get(dbRef).then((snapshot: DataSnapshot) => {
      let count = 0;
      snapshot.forEach(() => {
        count++;
      });
      return count;
    });
  }
  async getEntries(): Promise<void> {
    const database = getDatabase();
    const dbRef = ref(database, 'comments');
    const dbRefSubs = ref(database, 'subtitles');
    get(dbRef).then((snapshot: DataSnapshot) => {
      if (this.entries) {
        snapshot.forEach((childSnapshot) => {
          const comment = childSnapshot.val();
          comment.fbkey = childSnapshot.key;
          this.entries.push(comment);
        });
      }
    });
    get(dbRefSubs).then((snapshot: DataSnapshot) => {
      if (this.entriesSubtitleS) {
        snapshot.forEach((childSnapshot) => {
          const comment = childSnapshot.val();
          comment.fbkey = childSnapshot.key;
          this.entriesSubtitleS.push(comment);
        });
      }
    });
  }
  public dissmis(comment: any, subtitle: any) {
    if (comment != null) {
      const database = getDatabase();
      const commentRef = ref(database, 'comments/' + comment.fbkey);
      remove(commentRef);
    }
    else {
      const database = getDatabase();
      const commentRef = ref(database, 'subtitles/' + subtitle.fbkey);
      remove(commentRef);
    }
    setInterval(() => { window.location.reload(); }, 2000);
  }
  public delete(comment: any, subtitle: any) {
    if (comment != null) {
      // console.log(comment);
      this.adminServices.deleteComment(comment.commentID).subscribe({
        next: (response) => { console.log(response); },
        error: (err) => { console.log(`Neuspjesno brisanje!`); console.log(err); },
        complete: () => { console.log('Obrisano!'); window.location.reload(); }
      });
      this.dissmis(comment, null);
    }
    else {
      console.log(subtitle);
      if (!subtitle.isTVShow) {
        this.adminServices.deleteMovieSubtitle(subtitle.subMovieID, subtitle.uplaoder).subscribe({
          next: (response) => { console.log(response); },
          error: (err) => { console.log(`Neuspjesno brisanje!`); console.log(err); },
          complete: () => { console.log('Obrisano!'); window.location.reload(); }
        });
      }
      else {
        this.adminServices.deleteTVShowSubtitle(subtitle.subMovieID, subtitle.uplaoder, subtitle.numberEpisode, subtitle.numberSeason).subscribe({
          next: (response) => { console.log(response); },
          error: (err) => { console.log(`Neuspjesno brisanje!`); console.log(err); },
          complete: () => { console.log('Obrisano!'); window.location.reload(); }
        })
      }
      this.dissmis(null, subtitle);
    }
  }
  public getLangName(lang: string): string {

    switch (lang) {
      case "en": return "English";
      case "bs": return "Bosanski";
      case "de": return "Deutsche";
    }

    return "Unknown";
  }

  public switchLang(lang: string): void {

    this.translate.use(lang);
  }
  public notifications() {
    this.showNotifications = !this.showNotifications;
  }

  async searchTVShowOrMovie(): Promise<TVShowViewModel[] | MovieViewModel[]> {
    if (this.searchValue != '') {
      this.moviesService.searchedMovies = await new Promise((resolved) => {
        this.moviesService.getMovieByParameter(this.searchValue).subscribe({
          next: (data) => { resolved(data.body as MovieViewModel[]); this.moviesService.isMovieSearched = true; },
          error: () => { "Movie couldn't be searched!"; this.moviesService.isMovieSearched = false; }
        });
      });

      this.tvShowService.searchedTVShows = await new Promise((resolved) => {
        this.tvShowService.GetTVShow(this.searchValue).subscribe({
          next: (data) => {
            if (data.body != null) {
              resolved(data.body);
            }
            this.tvShowService.isTVShowSearched = true;
          },
          error: () => { console.log("TV Show couldn't be searched!"); this.tvShowService.isTVShowSearched = false; }
        });
      });

      if (this.moviesService.searchedMovies != null) {

        return this.moviesService.searchedMovies;
      } else {
        return this.tvShowService.searchedTVShows;
      }
    }
    this.moviesService.isMovieSearched = false;
    this.tvShowService.isTVShowSearched = false;
    return [];
  }

}