import { Component, OnInit } from '@angular/core';
import { Book } from '../models/Book';
import { ItemsService } from '../_services/items.service';
import { BasketService } from '../_services/basket.service';
import { BooksSignalrService } from '../_services/book-notification.service';
import { AccountService } from '../_services/account.service';
import { UserNotificationService } from '../_services/user-notification.service';

@Component({
  selector: 'app-goods',
  templateUrl: './goods.component.html',
  styleUrls: ['./goods.component.css']
})
export class GoodsComponent implements OnInit{
  
  books: Book[]
  userLoggedIn: boolean;

  constructor(private itemsService: ItemsService, private basketService: BasketService, 
      private booksNotificationService: BooksSignalrService, public accountService: AccountService, private userNotification: UserNotificationService){
    this.books = [];
    this.itemsService.books$.subscribe(books => this.books = books)
  }
  ngOnInit(): void {
    this.itemsService.getItems();
    this.booksNotificationService.initiateSignalRConnection();      
    }


  addToBasket(book: Book){
    var x = this.basketService.books.find(x => x.isbn === book.isbn)
    if( x === undefined){
      this.basketService.addToBasket(book);
      this.userNotification.SendOrderUpdates();
    }
    else{
      alert("Sorry, only one copy of the book is available for purchase.")
    }
      
  }
}
