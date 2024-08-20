import { Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { ChatBuilderService } from '../services/chat-builder.service';
import { Chat, User } from '../models/User';
import { AuthService } from '../services/auth.service';
import { ChatService } from '../services/chat.service';
import { MessageService } from '../services/message.service';
import { Subject, Subscription, firstValueFrom, takeUntil } from 'rxjs';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit{

  messageInput: string = '';
  @Output() remove: EventEmitter<any> = new EventEmitter()
  @Input() chat!: Chat
  private messageSubscription?: Subscription;
  private destroy$ = new Subject<void>();
  constructor(private renderer: Renderer2, public authService: AuthService, private messageService: MessageService,
    private chatService: ChatService) {

  }
  ngOnInit(): void {
    this.unsubscribeMessageService();
    this.messageSubscription = this.messageService.messageSubject$
      .pipe(takeUntil(this.destroy$))
      .subscribe(message => {
        if (message && message.chatId==this.chat.id) {
          if (!this.chat.messages) {
            this.chat.messages=[]
          }
        this.chat.messages.push({
          sendingDate:message.sendingDate,
          value: message.value,
          user:message.user
        });
      }
    })     
  }
  async leaveChat(chatId:string) {
    await this.messageService.hubConnection?.invoke("LeaveChat", chatId);
  }
  async joinChat(chatId:string) {
    await this.messageService.hubConnection?.invoke("JoinChat", chatId);
}

  ngOnDestroy(): void {
    this.unsubscribeMessageService();
    this.destroy$.next();
    this.destroy$.complete();
  }

  removeChat() {
    this.chat.messages = []; // Clear messages to avoid duplication when re-subscribing
    this.remove.emit(this.chat);
    this.messageService.messageReceivedSubject.next(null)
    this.unsubscribeMessageService();
    this.leaveChat(this.chat.id.toString())
  }

  private unsubscribeMessageService() {
    if (this.messageSubscription) {
      this.messageSubscription.unsubscribe();
      this.messageSubscription = undefined;
    }
  }

 async sendMessage() {
    if (this.messageInput.trim() != "") {
      const user: User | undefined = this.authService.getCurrentUser();

      if (user != undefined) {
        let createdChatID: number = this.chat.id;

        if (this.chat.id == 0) {
          createdChatID = await firstValueFrom(this.chatService.createChat({
            messages: [],
            users: this.chat.users,
            name: null,
            id: 0
          }))

          if (createdChatID===0) {
            throw new Error("you message has not been sent.")
            return;
          }
          this.joinChat(createdChatID.toString())

        }

        this.messageService.sendMessage({
          sendingDate: new Date(Date.now()),
          user: {
            email: user.email,
            userName: user.userName
          },
          value: this.messageInput,
          chatId: createdChatID
        });
        this.messageInput = '';
      }
     
    }
  }

}
