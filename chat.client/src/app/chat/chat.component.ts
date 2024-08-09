import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {

  @ViewChild("chatArea") elRef! : ElementRef
  constructor(private renderer: Renderer2) {

  }


  createChat(userName: string) {

  }
}
