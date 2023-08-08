import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { DirectMessageViewModel } from 'src/app/models/directmessage-viewmodel';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { DirectmessageService } from 'src/app/services/directmessage.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-page-direct-messages',
  templateUrl: './page-direct-messages.component.html',
  styleUrls: ['./page-direct-messages.component.css']
})
export class PageDirectMessagesComponent implements OnInit {

  private connection!: HubConnection;

  private isRecipientLoaded: boolean = false;
  private isUserOffline: boolean = false;

  public liveArrayOfMessages: string[] = [];

  public dataObject: DirectMessageViewModel = new DirectMessageViewModel();
  public dbDataObject: DirectMessageViewModel[] | null = [];

  public loggedUser: UserViewModel | null = null;
  public conversationsData: UserViewModel[] | null = [];

  public userConnectionId: string = "";
  public friendUsername: string = "";

  public shouldLoad: boolean = false;

  constructor(public userService: UserService, public directMessageService: DirectmessageService) {
    this.connection = new HubConnectionBuilder().withUrl(`${environment.baseAPIURL}/chat`).build();
  }

  async ngOnInit() {
    this.loggedUser = await this.userService.getCurrentUser();

    this.dataObject.userID = this.loggedUser?.id;

    this.connection.on('ReceiveMessage', (username: string, message: string) => {
      this.liveArrayOfMessages.push(`${username}: ${message}`);
    });

    try {
      await this.connection.start().then(async () => { await this.connection.invoke("GetConnectionID", this.loggedUser?.username).then((id: string) => this.userConnectionId = id) });
      console.log('Connected to SignalR hub!');
    } catch (error) {
      console.error('Failed to connect to SignalR hub', error);
    }

    this.directMessageService.GetAllConversations(this.dataObject.userID).subscribe({
      next: (response) => { this.conversationsData = response.body; },
      error: (err) => { console.log(err); }
    })

    this.shouldLoad = true;
  }

  async openUserConversation(arrayIndex: number) {

    let getMessage: DirectMessageViewModel[] = await new Promise((resolved) => {
      this.directMessageService.GetMessagesForUser(this.dataObject.userID, this.conversationsData?.at(arrayIndex)?.id).subscribe({
        next: (response) => {
          resolved(response.body as DirectMessageViewModel[]);
        },
        error: (err) => { console.log(err); }
      });
    });

    let getReceiverMessage: DirectMessageViewModel[] = await new Promise((resolved) => {
      this.directMessageService.GetMessagesForUser(this.conversationsData?.at(arrayIndex)?.id, this.dataObject.userID).subscribe({
        next: (response) => { resolved(response.body as DirectMessageViewModel[]); },
        error: (err) => { console.log(err); }
      });
    });

    this.liveArrayOfMessages = [];

    for (let i = 0; i < getMessage.length; i++) {
      let message = getMessage?.at(i)?.messageContent;

      let receiverMessage = getReceiverMessage?.at(i)?.messageContent;

      let receiverID = getMessage?.at(i)?.recipientUserID;

      if (receiverID != null) {
        this.userService.getUserByID(receiverID).subscribe({
          next: (response) => {
            if (response.body?.username != undefined) {
              this.friendUsername = '';
              this.friendUsername = response.body?.username
            }
          },
          error: (err) => { console.log(err); }
        })
      }

      let receiverName: UserViewModel = await new Promise((resolved) => {
        if (receiverID != null) {
          this.userService.getUserByID(receiverID).subscribe({
            next: (response) => {
              resolved(response.body as UserViewModel);
            },
            error: (err) => { console.log(err); }
          });
        }
      });

      if (message != null && receiverID != null) {
        this.liveArrayOfMessages.push(`${this.loggedUser?.username}: ${message}`);
        if (getReceiverMessage.at(i) != null) {
          this.liveArrayOfMessages.push(`${receiverName.username}: ${receiverMessage}`);
        }
      }
    }
  }

  async findRecipientUser() {
    this.userService.getUserByEmail(undefined, this.friendUsername).subscribe({
      next: (response) => {
        this.dataObject.recipientUserID = response.body?.id;
        this.isRecipientLoaded = true;
        // console.log(response);
      },
      error: (err) => {
        this.isRecipientLoaded = false;
      }
    });
  }

  async SendToUser() {
    // console.log(`${this.loggedUser?.username} - ${this.friendUsername} - ${this.dataObject.messageContent}`);

    if (!this.loggedUser?.username || !this.dataObject.messageContent || !this.friendUsername) {
      console.error("Error sending a message!");
      return;
    }

    await this.connection.invoke<boolean>('SendToUser', this.loggedUser?.username, this.friendUsername, this.dataObject.messageContent).catch(() => { this.isUserOffline = true });

    if (this.isRecipientLoaded && !this.isUserOffline) {
      this.directMessageService.SendMessage(this.dataObject).subscribe({
        next: async (response) => {
          // console.log(response);
          this.liveArrayOfMessages.push(`${this.loggedUser?.username}: ${this.dataObject.messageContent}`);
          this.dataObject.messageContent = '';
        },
        error: (err) => { console.log(err); }
      })
    } else if (this.isRecipientLoaded && this.isUserOffline) {
      this.directMessageService.SendMessage(this.dataObject).subscribe({
        next: async (response) => {
          // console.log(response);
          this.dataObject.messageContent = '';
        },
        error: (err) => { console.log(err); }
      })

      this.liveArrayOfMessages.push(`${this.loggedUser?.username}: ${this.dataObject.messageContent}`);
      this.dataObject.messageContent = '';
    }
  }
}