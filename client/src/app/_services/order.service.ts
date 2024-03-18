import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  orderUrl$ = new BehaviorSubject<string>(null);
  url: string;
  constructor() {
    this.orderUrl$.subscribe(x => this.url = x)
  }
}
