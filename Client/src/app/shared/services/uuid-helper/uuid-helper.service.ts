import { Injectable } from '@angular/core';
import ShortUniqueId from 'short-uuid';

@Injectable({
  providedIn: 'root'
})
export class UuidHelperService {
  private translator = ShortUniqueId();

  toShortUuid(uuid: string): string {
    return this.translator.fromUUID(uuid);
  }

  toUuid(shortUuid: string | null): string {
    if (!shortUuid) {
      return '';
    }
    
    return this.translator.toUUID(shortUuid);
  }
}
