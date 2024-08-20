import { Component } from '@angular/core';
import { Chat, ChatDetails, Message, SearchResult, User } from '../models/User';
import { ChatService } from '../services/chat.service';
import { firstValueFrom } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.css'
})
export class ChatListComponent {
  chats: Chat[] =[]

  constructor(private chatService: ChatService, private authService:AuthService) {
    this.authService.initialize();
  }
  async getMessagesAndChatId(searchResult: SearchResult): Promise<ChatDetails|undefined>{
    try {
      const userLoggedIn: User | undefined = this.authService.getCurrentUser();
      if (userLoggedIn != undefined) {
        searchResult.users.push(userLoggedIn)
      }
      //tutaj musi byc zwrot tego obiektu z chatid i messages[]
      const chatDetails: ChatDetails = await firstValueFrom(this.chatService.getMessagesFromChatAndChatId(searchResult));

      return chatDetails;
    } catch (error) {
      console.error('Error fetching messages', error);
      return undefined; // or handle the error as appropriate
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

      const chatDetails: ChatDetails | undefined = await this.getMessagesAndChatId(searchResult);
    if (chatDetails != undefined) {

      this.chats.push({
        name: searchResult.name,
        users: searchResult.users,
        messages: chatDetails.messages,
        id: chatDetails.chatId
      });
    }
    else {
      console.log("error while sending message")
    }
    }
}
