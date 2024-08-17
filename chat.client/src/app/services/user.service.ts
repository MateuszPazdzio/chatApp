import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginResponse, LoginUserCred, RegisterResponse, RegisterUserCred, SearchResult, User } from '../models/User';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  //checkIfUserWithSpecificEmailOrUserNameExists(searchPhrase: any): Observable<Status> {
  //  return this.httpClient.get<Status>("https://localhost:7282/api/chat");
  //}

  constructor(private httpClient: HttpClient) { }

  getProposedChatConversationsBySearchInput(userName: string): Observable<SearchResult[]> {
    let params = new HttpParams();
    params=params.append("searchPhrase", userName);
    return this.httpClient.get<SearchResult[]>('https://localhost:7282/api/chat', {params});
  }

  loginUser(userLoginCred: LoginUserCred) : Observable<LoginResponse>{
    return this.httpClient.post<LoginResponse>("https://localhost:7282/api/auth/login", userLoginCred);
  }

  registerUser(userRegisterCred: RegisterUserCred): Observable<RegisterResponse> {
    return this.httpClient.post<RegisterResponse>("https://localhost:7282/api/auth/register", userRegisterCred);
  }

  logoutUser() {
    return this.httpClient.post<any>("https://localhost:7282/api/auth/logout", {});
  }
}
