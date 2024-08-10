import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SearchResult, User } from '../models/User';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getProposedChatConversationsBySearchInput(userName: string): Observable<SearchResult[]> {
    let params = new HttpParams();
    params=params.append("searchPhrase", userName);
    return this.httpClient.get<SearchResult[]>('https://localhost:7282/api/chat', {params});
  }
}
