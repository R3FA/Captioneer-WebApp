import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-subtitle-page',
  templateUrl: './subtitle-page.component.html',
  styleUrls: ['./subtitle-page.component.css']
})
export class SubtitlePageComponent implements OnInit {

  constructor() { }

  myControl = new FormControl('');
  subtitles: string[] = ['English', 'Bosnian', 'French', 'Norwegian'];
  filteredOptions!: Observable<string[]>;

  ngOnInit() {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.subtitles.filter(option => option.toLowerCase().includes(filterValue));
  }

  isMenuOpened: boolean = false;

  toggleMenu(): void {
    this.isMenuOpened = !this.isMenuOpened;
  }

  clikedOutside(): void {
    this.isMenuOpened = false;
  }

}
