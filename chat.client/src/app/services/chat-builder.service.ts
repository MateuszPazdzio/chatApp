import { ElementRef, Injectable, Renderer2, ViewChild } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ChatBuilderService {

  @ViewChild("chatArea") elRef!: ElementRef
  constructor(private renderer: Renderer2, private el: ElementRef) {

  }

  buildChat() {
    // Get reference to the chatArea container
    const chatArea = this.el.nativeElement.querySelector('.chatArea');

    // Create the chat container
    const chat = this.renderer.createElement('div');
    this.renderer.addClass(chat, 'chat');
    this.renderer.setAttribute(chat, 'id', 'chat1');

    // Create the chat head
    const chatHead = this.renderer.createElement('div');
    this.renderer.addClass(chatHead, 'head');
    this.renderer.addClass(chatHead, 'd-flex');
    this.renderer.addClass(chatHead, 'justify-content-between');
    this.renderer.addClass(chatHead, 'chatHead');

    // Create the title container
    const titleContainer = this.renderer.createElement('div');
    this.renderer.addClass(titleContainer, 'title');
    this.renderer.addClass(titleContainer, 'col');

    // Create the title text
    const titleText = this.renderer.createElement('p');
    this.renderer.addClass(titleText, 'text');
    const titleContent = this.renderer.createText('Mateusz Paździoffffffffffffffffffff');
    this.renderer.appendChild(titleText, titleContent);
    this.renderer.appendChild(titleContainer, titleText);

    // Create the close button container
    const closeBtnContainer = this.renderer.createElement('div');
    this.renderer.addClass(closeBtnContainer, 'closeBtn');

    // Create the close button
    const closeBtn = this.renderer.createElement('button');
    this.renderer.addClass(closeBtn, 'btn');
    this.renderer.addClass(closeBtn, 'btn-dark');
    this.renderer.setAttribute(closeBtn, 'type', 'button');
    const closeBtnText = this.renderer.createText('X');
    this.renderer.appendChild(closeBtn, closeBtnText);
    this.renderer.appendChild(closeBtnContainer, closeBtn);

    // Append the title and close button to the chat head
    this.renderer.appendChild(chatHead, titleContainer);
    this.renderer.appendChild(chatHead, closeBtnContainer);

    // Create the chat body
    const chatBody = this.renderer.createElement('div');
    this.renderer.addClass(chatBody, 'body');

    // Create the message area
    const messageArea = this.renderer.createElement('div');
    this.renderer.addClass(messageArea, 'messageArea');
    this.renderer.addClass(messageArea, 'd-flex');
    this.renderer.addClass(messageArea, 'flex-column');

    // Create the input area
    const inputArea = this.renderer.createElement('div');
    this.renderer.addClass(inputArea, 'inputArea');
    this.renderer.addClass(inputArea, 'form-group');

    // Create the input control
    const inputControl = this.renderer.createElement('input');
    this.renderer.addClass(inputControl, 'input-control');
    this.renderer.setAttribute(inputControl, 'type', 'text');
    this.renderer.setAttribute(inputControl, 'name', 'message');
    this.renderer.setAttribute(inputControl, 'placeholder', 'Message..');

    // Create the send button
    const sendBtn = this.renderer.createElement('input');
    this.renderer.addClass(sendBtn, 'btn');
    this.renderer.addClass(sendBtn, 'btn-primary');
    this.renderer.setAttribute(sendBtn, 'type', 'button');
    this.renderer.setAttribute(sendBtn, 'name', 'sendingBtn');
    this.renderer.setAttribute(sendBtn, 'value', '>');

    // Append input control and send button to input area
    this.renderer.appendChild(inputArea, inputControl);
    this.renderer.appendChild(inputArea, sendBtn);

    // Append message area and input area to the chat body
    this.renderer.appendChild(chatBody, messageArea);
    this.renderer.appendChild(chatBody, inputArea);

    // Append the chat head and body to the chat container
    this.renderer.appendChild(chat, chatHead);
    this.renderer.appendChild(chat, chatBody);

    // Finally, append the chat container to the chatArea
    this.renderer.appendChild(chatArea, chat);
  }

}
