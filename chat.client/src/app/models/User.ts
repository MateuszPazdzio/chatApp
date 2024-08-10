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
  name:string,
  users: User[],
  messages:Message[]
}


export interface SearchResult {
  users:User[]
  name:string
}
