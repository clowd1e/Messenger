import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private config: any;

  async loadConfig(): Promise<void> {
    const res = await fetch('environment.json');
    const cfg = await res.json();
    return this.config = cfg;
  }

  get<T>(key: string): T {
    if (!this.config) {
      throw new Error('Config not loaded. Call loadConfig() first.');
    }
    const value = this.config[key];
    if (value === undefined) {
      throw new Error(`Config key "${key}" not found.`);
    }
    return value as T;
  }
}
