import { Component, ElementRef, EventEmitter, Input, Output, Renderer2, ViewChild } from '@angular/core';
import { ChatBuilderService } from '../services/chat-builder.service';
import { Chat } from '../models/User';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent {


  @Output() remove: EventEmitter<any> = new EventEmitter()
  @Input() chat!:Chat
  constructor(private renderer:Renderer2) {

  }

  removeChat() {
    this.remove.emit(this.chat);
  }
}
