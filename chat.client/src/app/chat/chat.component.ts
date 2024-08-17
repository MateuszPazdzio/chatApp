import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { ChatBuilderService } from '../services/chat-builder.service';
import { Chat, User } from '../models/User';
import { AuthService } from '../services/auth.service';
import { ChatService } from '../services/chat.service';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit, OnDestroy{

  messageInput: string = '';
  @Output() remove: EventEmitter<any> = new EventEmitter()
  @Input() chat!:Chat
  constructor(private renderer:Renderer2,public authService:AuthService,private  messageService:MessageService) {

  }

  ngOnDestroy(): void {
    this.messageService.hubConnection?.off("ReceiveMessage")
  }

  ngOnInit(): void {
    this.messageService.messageSubject$.subscribe(message => {
      console.log(message)
      //if (message) {
      //  this.chat.messages.push(message);
      //}
    })     
  }

  removeChat() {
    this.remove.emit(this.chat);
  }

  sendMessage() {
    if (this.messageInput.trim() != "") {
      const user: User | undefined = this.authService.getCurrentUser();
      if (user != undefined) {
        this.messageService.sendMessage({
          time: new Date(Date.now()),
          user: {
            email: user.email,
            userName: user.userName
          },
          value: this.messageInput
        });
        this.messageInput = '';
      }
     
    }
  }
}
