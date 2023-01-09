import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-subtitle-page',
  templateUrl: './subtitle-page.component.html',
  styleUrls: ['./subtitle-page.component.css']
})
export class SubtitlePageComponent implements OnInit {

  constructor() { }
  isAdmin: boolean = false;
  isTVSeries: boolean = false;
  ngOnInit() { }
}
