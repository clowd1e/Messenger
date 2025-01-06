import { TestBed } from '@angular/core/testing';

import { UuidHelperService } from './uuid-helper.service';

describe('UuidHelperService', () => {
  let service: UuidHelperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UuidHelperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
