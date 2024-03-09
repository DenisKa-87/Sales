import { Component, OnInit } from '@angular/core';
import { Book } from '../models/Book';
import { ItemsService } from '../_services/items.service';
import { BasketService } from '../_services/basket.service';
import { SignalrService } from '../_services/signalr.service';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-goods',
  templateUrl: './goods.component.html',
  styleUrls: ['./goods.component.css']
})
export class GoodsComponent implements OnInit{
  
  books: Book[]
  userLoggedIn: boolean;

  constructor(private itemsService: ItemsService, private basketService: BasketService, 
      private signalrService: SignalrService, public accountService: AccountService){
    this.books = [];
    this.itemsService.books$.subscribe(books => this.books = books)
  }
  ngOnInit(): void {
    this.itemsService.getItems();
    this.signalrService.initiateSignalRConnection();      
    }


  addToBasket(book: Book){
    var x = this.basketService.books.find(x => x.isbn === book.isbn)
    console.log(x);
    if( x === undefined){
      this.basketService.addToBasket(book);
    }
    else{
      alert("Sorry, only one copy of the book is available for purchase.")
    }
      
  }
}
