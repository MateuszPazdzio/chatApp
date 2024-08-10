import { Component, ElementRef, EventEmitter, Output, Renderer2, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { SearchResult } from '../models/User';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-search-list',
  templateUrl: './search-list.component.html',
  styleUrl: './search-list.component.css'
})
export class SearchListComponent {
  searchResults: SearchResult[] = []
  @Output() createChat: EventEmitter<any> = new EventEmitter();
  @ViewChild("userNameInputValue") userNameInputValue!: ElementRef;
  constructor(private userService: UserService, private renderer: Renderer2) {

  }

  sendToChatComponent(searchResult:any) {
    this.createChat.emit(searchResult);
  }

  onKeyUp(key: Event): void {
    let keyyboardEvent: KeyboardEvent = key as KeyboardEvent;
    this.userService.getProposedChatConversationsBySearchInput(this.userNameInputValue.nativeElement.value).subscribe(
      data => {
        this.searchResults = data
        if (this.searchResults.length > 0) {
          console.log("search results > 0")
          //this.createDropDownListWithValidUserNames()
        }
        else {
          this.removeAllUserNamesCurrentlyListed();
        }

      },
      error => {
        console.log("error while fetching data")
      }
    )
  }
  removeAllUserNamesCurrentlyListed() {
    let userNameItem = this.searchResults=[];
    //while (userNameItem.firstChild) {
    //  this.renderer.removeChild(userNameItem, userNameItem.firstChild);
    //}
  }
  onSubmit(form: NgForm) {
    if (form.valid) {
      this.userService.getProposedChatConversationsBySearchInput(form.value["userName"]).subscribe(
        data => {
          this.searchResults = data
          console.log(this.searchResults)
          //if (this.searchResults.length > 0) {
          //  this.createDropDownListWithValidUserNames()
          //}

        },
        error => {
          console.log("error while fetching data")
        }
      )
    }


  }
}
