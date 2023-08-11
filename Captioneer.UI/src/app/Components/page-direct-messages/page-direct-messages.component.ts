import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { DirectMessageViewModel } from 'src/app/models/directmessage-viewmodel';
import { UserViewModel } from 'src/app/models/user-viewmodel';
import { DirectmessageService } from 'src/app/services/directmessage.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

interface MessageContainer {
  timeStamp?: Date;
  stringTimeStamp?: string | null;
  userName: string;
  contentMessage: string;
}

@Component({
  selector: 'app-page-direct-messages',
  templateUrl: './page-direct-messages.component.html',
  styleUrls: ['./page-direct-messages.component.css'],
  providers: [DatePipe]
})
export class PageDirectMessagesComponent implements OnInit {

  private connection!: HubConnection;

  private isRecipientLoaded: boolean = false;
  private isUserOffline: boolean = false;

  public liveArrayOfMessages: MessageContainer[] = [];

  public dataObject: DirectMessageViewModel = new DirectMessageViewModel();
  public dbDataObject: DirectMessageViewModel[] | null = [];

  public loggedUser: UserViewModel | null = null;
  public conversationsData: UserViewModel[] | null = [];

  public userConnectionId: string = "";
  public friendUsername: string = "";
  public receiverMessageTimeFormated: string | null = '';
  public userMessageTimeFormated: string | null = '';

  public shouldLoad: boolean = false;

  constructor(public userService: UserService, public directMessageService: DirectmessageService, public datePipe: DatePipe) {
    this.connection = new HubConnectionBuilder().withUrl(`${environment.baseAPIURL}/chat`).build();
  }

  async ngOnInit() {
    this.loggedUser = await this.userService.getCurrentUser();

    this.dataObject.userID = this.loggedUser?.id;

    this.connection.on('ReceiveMessage', (username: string, message: string) => {
      // this.liveArrayOfMessages.push(`${username}: ${message}`);
      this.liveArrayOfMessages.push({
        userName: `${username}: `,
        contentMessage: message
      });
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

      let userMessageTime = getMessage.at(i)?.timeSent;

      this.userMessageTimeFormated = this.datePipe.transform(userMessageTime, 'dd.MM.yyyy / HH.mm.ss');

      if (message != null && this.userMessageTimeFormated && this.loggedUser != null) {
        // this.liveArrayOfMessages.push(this.userMessageTimeFormated, `${this.loggedUser?.username}: ${message}`);
        this.liveArrayOfMessages.push({
          timeStamp: userMessageTime,
          stringTimeStamp: this.userMessageTimeFormated,
          userName: this.loggedUser?.username,
          contentMessage: message
        });
      }
    }

    for (let i = 0; i < getReceiverMessage.length; i++) {

      let receiverID = getMessage?.at(i)?.recipientUserID;

      let receiverMessage = getReceiverMessage?.at(i)?.messageContent;

      let receiverMessageTime = getReceiverMessage.at(i)?.timeSent;

      this.receiverMessageTimeFormated = this.datePipe.transform(receiverMessageTime, 'dd.MM.yyyy / HH.mm.ss');

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

      if (receiverMessage != null && this.receiverMessageTimeFormated != null) {
        // this.liveArrayOfMessages.push(this.receiverMessageTimeFormated, `${receiverName.username}: ${receiverMessage}`);
        this.liveArrayOfMessages.push({
          timeStamp: receiverMessageTime,
          stringTimeStamp: this.receiverMessageTimeFormated,
          userName: receiverName.username,
          contentMessage: receiverMessage
        });
      }
      this.liveArrayOfMessages.sort((a: MessageContainer, b: MessageContainer) => {
        if (a.timeStamp! < b.timeStamp!) {
          return -1;
        }
        return 1;
      });
    }
    // console.log(this.liveArrayOfMessages);
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
          // this.liveArrayOfMessages.push(`${this.loggedUser?.username}: ${this.dataObject.messageContent}`);
          if (this.loggedUser?.username != null && this.dataObject.messageContent != null) {
            this.liveArrayOfMessages.push({
              userName: this.loggedUser.username,
              contentMessage: this.dataObject.messageContent
            });
          }
          console.log(this.liveArrayOfMessages);
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

      // this.liveArrayOfMessages.push(`${this.loggedUser.username}: ${this.dataObject.messageContent}`);
      this.liveArrayOfMessages.push({
        userName: this.loggedUser.username,
        contentMessage: this.dataObject.messageContent
      })
      this.dataObject.messageContent = '';
    }
  }
}