import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Book } from '../models/Book';
import { BasketService } from '../_services/basket.service';
import { AccountService } from '../_services/account.service';
import { SignalrService } from '../_services/signalr.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit{

  books: Book[];

  constructor(private basketService: BasketService, public accountService: AccountService, private signalrService: SignalrService){
    this.books = [];
    this.basketService.books$.subscribe(x => this.books = x);
  }
  ngOnInit(): void {
    if(localStorage.getItem('user') !== null)
      this.basketService.getCurrentOrder();
  }
    
  removeFromBasket(book: Book){
    this.basketService.removeFromBasket(book);
  }

}
