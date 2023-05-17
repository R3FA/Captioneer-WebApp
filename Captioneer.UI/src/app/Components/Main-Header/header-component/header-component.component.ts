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
import { ref, set, push, onValue, get, DataSnapshot,remove } from 'firebase/database';
import { initializeApp } from 'firebase/app';

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
  numberOfEntries!:number;
  showNotifications:boolean=false;
  entries: any[]=[];

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
    public userLanguageService: UserlanguageService, public route: ActivatedRoute) {
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
    await this.getNumberOfEntries().then(numEntries => {
      this.numberOfEntries = numEntries;
      console.log('Number of entries:', this.numberOfEntries);
    }).catch(error => {
      console.error('Error getting number of entries:', error);
    });
    await this.getEntries();
  }

  async gotoMyProfile() {
    let selectedUser = await this.userService.getCurrentUser();
    this.router.navigate([`Profile/`, `${selectedUser?.id}`]);
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

  async getNumberOfEntries(): Promise<number> {
    const database = getDatabase();
    const dbRef = ref(database);
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
    const dbRef = ref(database);
    get(dbRef).then((snapshot: DataSnapshot) => {
      if (this.entries) {
        snapshot.forEach((childSnapshot) => {
          const comment = childSnapshot.val();
          comment.fbkey = childSnapshot.key;
          this.entries.push(comment);
        });
      }
      console.log(this.entries);
    });
  }
  public dissmis(comment:any){
    const database = getDatabase();
    const commentRef = ref(database, comment.fbkey);
    remove(commentRef);
    window.location.reload();
  }
  public delete(comment:any){

    //Fare ostavi dissmis funckiju samo na kraju. Ti svoj kod gore napisi iznad ovog komentara.
    this.dissmis(comment);
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
  public notifications(){
      this.showNotifications=!this.showNotifications;
  }


}
