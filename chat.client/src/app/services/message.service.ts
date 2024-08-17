import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Message } from '../models/User';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  public hubConnection?: signalR.HubConnection;
  //private messageReceivedSubject = new BehaviorSubject<Message | null>(null);
  private messageReceivedSubject = new BehaviorSubject<string|null>(null);
  messageSubject$ = this.messageReceivedSubject.asObservable();
  constructor() {
    this.startConnection();
    this.addMessageListener();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7282/chatHub', {skipNegotiation:true, transport:signalR.HttpTransportType.WebSockets})  // Replace with your SignalR hub URL
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .withHubProtocol(new signalR.JsonHubProtocol())
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
    this.hubConnection.serverTimeoutInMilliseconds = 60000
  }

  private addMessageListener() {
    //this.hubConnection?.on('ReceiveMessage', (message: Message) => {
    //  this.messageReceivedSubject.next(message);
    //});
    this.hubConnection?.on('ReceiveMessage', (message: string|null) => {
      this.messageReceivedSubject.next(message);
    });
  }

  public sendMessage(message: Message) {
    this.hubConnection?.invoke('SendMessage', message.value)
      .catch(err => console.error(err));
  }

}
