import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-sign-up-done',
  templateUrl: './sign-up-done.component.html',
  styleUrls: ['./sign-up-done.component.css']
})
export class SignUpDoneComponent implements OnInit {

  constructor(private dialogRef:MatDialog) { }

  ngOnInit(): void {
  }
  onClose(){
    this.dialogRef.closeAll();
  }
}
