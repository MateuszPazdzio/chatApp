import { Component, ElementRef, EventEmitter, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/User';

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrl: './search-user.component.css'
})
export class SearchUserComponent implements OnInit {
  @ViewChild('userNameInputValue') userNameInputValue!:ElementRef;
  proposedUsers!: User[]
  @ViewChild("userNameList") userNameDivList!: ElementRef;

  @Output() createChat: EventEmitter<string> = new EventEmitter<string>();
  OnClickOnUserNameFromProposedList(userName: string) {
    this.createChat.emit(userName);
  }
  constructor(private userService: UserService, private renderer: Renderer2) {

  }

  ngOnInit(): void {

  }

  onKeyUp(key: Event): void {
    let keyyboardEvent: KeyboardEvent = key as KeyboardEvent;
    this.userService.getProposedUsersByUserName(this.userNameInputValue.nativeElement.value).subscribe(
      data => {
        this.proposedUsers = data
        if (this.proposedUsers.length > 0) {
          this.createDropDownListWithValidUserNames()
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

  onSubmit(form: NgForm) {
    if (form.valid) {
       this.userService.getProposedUsersByUserName(form.value["userName"]).subscribe(
        data => {
           this.proposedUsers = data
           console.log(this.proposedUsers)
           if (this.proposedUsers.length > 0) {
            this.createDropDownListWithValidUserNames()
           }

         },
         error => {
           console.log("error while fetching data")
         }
      )
    }


  }

  createDropDownListWithValidUserNames() {
    this.removeAllUserNamesCurrentlyListed();
    const divWrapper = this.renderer.createElement("div");
    this.renderer.addClass(divWrapper, "d-flex")
    this.renderer.addClass(divWrapper, "flex-column")

    for(const user of this.proposedUsers) {
      const userElement = this.renderer.createElement('a');
      this.renderer.listen(userElement, 'click', (event) => {
        this.OnClickOnUserNameFromProposedList(event.target.innerText);
      });
      this.renderer.addClass(userElement, "userNameFromProposedList");
      const userText = this.renderer.createText(user.userName);
      this.renderer.appendChild(userElement, userText);
      this.renderer.appendChild(divWrapper, userElement);
    }
    this.renderer.appendChild(this.userNameDivList.nativeElement, divWrapper)
  }
  removeAllUserNamesCurrentlyListed() {
    let userNameItem = this.userNameDivList.nativeElement;
    while (userNameItem.firstChild) {
      this.renderer.removeChild(userNameItem, userNameItem.firstChild);
      }
    }

}


