import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Message, SearchResult } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private httpClient: HttpClient) {

  }

  getMessagesFromChat(searchResult: SearchResult): Observable<Message[]>{

    return this.httpClient.post<Message[]>("https://localhost:7282/api/chat/messages", searchResult);
  }
}
