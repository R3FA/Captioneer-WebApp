import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-page-direct-messages',
  templateUrl: './page-direct-messages.component.html',
  styleUrls: ['./page-direct-messages.component.css']
})
export class PageDirectMessagesComponent implements OnInit {

  private connection!: HubConnection;
  public arrayMessages: string[] = [];
  public message: string = '';
  public loggedUser: UserViewModel | null = null;
  public userConnectionId: string = "";
  public friendUsername: string = "";

  constructor(public userService: UserService) {
    this.connection = new HubConnectionBuilder().withUrl(`${environment.baseAPIURL}/chat`).build();
  }

  async ngOnInit() {
    this.loggedUser = await this.userService.getCurrentUser();
    this.connection.on('ReceiveMessage', (username: string, message: string) => {
      this.arrayMessages.push(`${username}: ${message}`);
    });

    try {
      await this.connection.start().then(async () => { await this.connection.invoke("GetConnectionID", this.loggedUser?.username).then((id: string) => this.userConnectionId = id) });
      console.log('Connected to SignalR hub!');
      // console.log(this.userConnectionId);
      // console.log(this.loggedUser?.username);
    } catch (error) {
      console.error('Failed to connect to SignalR hub', error);
    }
  }

  async SendToUser(loggedUsername: string, userConnectionId: string, message: string) {
    console.log(`${loggedUsername} - ${this.friendUsername} - ${message}`);
    if (!loggedUsername || !message || !this.friendUsername) { console.error("Error sending a message!"); return; }
    await this.connection.invoke<boolean>('SendToUser', loggedUsername, this.friendUsername, message);
    this.arrayMessages.push(`${loggedUsername}: ${message}`);
    this.message = '';
  }
}