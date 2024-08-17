export interface User {
  userName: string,
  email:string
}

export interface Message{
  user: User
  time: Date,
  value:string
}
export interface Chat {
  name:string|null,
  users: User[],
  messages:Message[]
}


export interface SearchResult {
  users:User[]
  name:string|null
}

export interface LoginUserCred {
  userName: string,
  password:string
}


export interface RegisterUserCred extends User {
  login: string,
  password: string,
  passwordVer:string
}



export interface RegisterResponse {
  isRegistered: boolean,
  responseMessage:string
}

export interface LoginResponse {
  isLoggedIn: boolean,
  responseMessage: string,
  token:string
}

export interface DecodedToken {
  emailaddress?: string;
  name?: string;
  sub?: string;
}

export interface CurrentUser {
  user?: User,
  isAuthenticated:boolean
}
