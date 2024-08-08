import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/User';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getProposedUsersByUserName(userName: string): Observable<User[]> {
    let params = new HttpParams();
    params=params.append("userName", userName);
    return this.httpClient.get<User[]>('https://localhost:7282/api/chat', {params});
  }
}
