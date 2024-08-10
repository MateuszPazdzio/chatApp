import { TestBed } from '@angular/core/testing';

import { ChatBuilderService } from './chat-builder.service';

describe('ChatBuilderService', () => {
  let service: ChatBuilderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatBuilderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
