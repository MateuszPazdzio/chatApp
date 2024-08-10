import { Component } from '@angular/core';
import { Chat, Message, SearchResult } from '../models/User';
import { ChatService } from '../services/chat.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.css'
})
export class ChatListComponent {
  chats: Chat[] =[]

  constructor(private chatService: ChatService) {

  }
  async getMessages(searchResult: SearchResult) :Promise<Message[]>{
    try {
      const messages: Message[] = await firstValueFrom(this.chatService.getMessagesFromChat(searchResult));
      return messages;
    } catch (error) {
      console.error('Error fetching messages', error);
      return []; // or handle the error as appropriate
    }
  }

  removeChat(e: any) {
    this.chats.splice(this.chats.indexOf(e),1)
  }


  async createChat(searchResult: SearchResult) {
    if (this.chats.length > 0) {
      let chatExists = this.chats.filter(chat => chat.name === searchResult.name);
      if (chatExists.length > 0) {
        alert("chat exists");
        return;
      }
    }

    this.chats.push({
      name: searchResult.name,
      users: searchResult.users,
      messages:await this.getMessages(searchResult)
      //messages: [{
      //  time: new Date(),
      //  user: {
      //    email: "mail@wp.pl",
      //    userName: searchResult.name
      //  },
      //  value:"test msg"
      //}]
    })
    }
}
