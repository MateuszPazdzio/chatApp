import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SearchResult } from '../models/User';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrl: './search-result.component.css'
})
export class SearchResultComponent {
  @Input()
  searchResult!: SearchResult
  @Output()
  sendToChat: EventEmitter<SearchResult> = new EventEmitter<SearchResult>();

  OnClickOnUserNameFromProposedList() {
    this.sendToChat.emit(this.searchResult);
  }
}
