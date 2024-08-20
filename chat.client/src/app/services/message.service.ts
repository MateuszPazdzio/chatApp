import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Message, SendingMessage } from '../models/User';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  public hubConnection?: signalR.HubConnection;
  //private messageReceivedSubject = new BehaviorSubject<Message | null>(null);
  messageReceivedSubject = new BehaviorSubject<SendingMessage |null>(null);
  messageSubject$ = this.messageReceivedSubject.asObservable();
  constructor() {
    this.startConnection();
    this.addMessageListener();
    this.addUserJoinListener();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7282/chatHub', {skipNegotiation:true, transport:signalR.HttpTransportType.WebSockets})  // Replace with your SignalR hub URL
      .configureLogging(signalR.LogLevel.Debug) 
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .withHubProtocol(new signalR.JsonHubProtocol())
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
    this.hubConnection.serverTimeoutInMilliseconds = 60000
    this.hubConnection.keepAliveIntervalInMilliseconds=10000
    this.hubConnection.onclose(error => {
      console.log("Connection closed with error: ", error);
      // Handle reconnection logic here
    });

  }
  private addUserJoinListener() {
    this.hubConnection?.on("UserJoined", (msg: string) => {
      console.log(msg)
    })
  }
  private addMessageListener() {
    this.hubConnection?.on('ReceiveMessage', (message: SendingMessage) => {
      this.messageReceivedSubject.next(message);
    });
    //this.hubConnection?.on('ReceiveMessage', (message: string|null) => {
    //  this.messageReceivedSubject.next(message);
    //});
  }

  public sendMessage(message: SendingMessage) {
    this.hubConnection?.invoke('SendMessage', message)
      .catch(err => console.error(err));
  }

}
