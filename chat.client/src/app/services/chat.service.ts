import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Chat, ChatDetails, Message, SearchResult } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  createChat(chat:Chat) :Observable<number>{
    return this.httpClient.post<number>("https://localhost:7282/api/chat/create", chat);
  }

  constructor(private httpClient: HttpClient) {

  }

  getMessagesFromChatAndChatId(searchResult: SearchResult): Observable<ChatDetails>{
    //niech zwarac obiekt z chateid:number i messages[]
    return this.httpClient.post<ChatDetails>("https://localhost:7282/api/chat/messages", searchResult);
  }
}
