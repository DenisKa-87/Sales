import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Book } from '../models/Book';
import { BasketService } from '../_services/basket.service';
import { AccountService } from '../_services/account.service';
import { BooksSignalrService } from '../_services/book-notification.service';
import { BehaviorSubject } from 'rxjs';
import { UserNotificationService } from '../_services/user-notification.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit{

  books: Book[];
  orderPlaced = false;

  constructor(public basketService: BasketService, public accountService: AccountService, private usernotificationService: UserNotificationService){
    this.books = [];
    this.basketService.books$.subscribe(x => this.books = x);
    this.basketService.orderPlaced$.subscribe(placed => this.orderPlaced = placed);
  }
  ngOnInit(): void {
    if(localStorage.getItem('user') !== null){
      this.basketService.getCurrentOrder();
      this.usernotificationService.initiateSignalRConnection();
    }
      
  }
    
  removeFromBasket(book: Book){
    this.basketService.removeFromBasket(book);
    this.usernotificationService.SendOrderUpdates();
  }

}
