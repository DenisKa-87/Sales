import { Injectable} from '@angular/core';
import { Book } from '../models/Book';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Order } from '../models/Order';
import { BooksSignalrService } from './book-notification.service';
import { OrderService } from './order.service';
@Injectable({
  providedIn: 'root'
})

export class BasketService {

  books$ = new BehaviorSubject<Book[]>([]);
  books: Book[] = [];
  orderSum$ = new BehaviorSubject<number>(0);
  orderPlaced$ = new BehaviorSubject<boolean>(false);

  
  constructor(private http: HttpClient, private signalRService: BooksSignalrService, private orederService: OrderService) 
  { 
    this.books$.subscribe(books => this.books = books)

  }

  getCurrentOrder(){
    return this.http.get<Order>(environment.apiUrl + 'order/current', {observe: 'response'})
    .subscribe(response => {
      if(response.body.books !== null)
        this.books$.next(response.body.books);
      var sum = 0;
      response.body.books.forEach(book => sum += book.price)
      this.orderSum$.next(sum)
      this.orderPlaced$.next(response.body.placed)
      this.orederService.orderUrl$.next(response.body.orderUrl.length == 0 ? null : response.body.orderUrl)
    });
  }

  addToBasket(book: Book){
    if(book.available <= 0){
      alert("Sorry, this book is no longer available.")
      return;
    }
    return this.http.post<Order>(environment.apiUrl + "order/addBook/" +book.isbn, {}).subscribe( resp => {
      if(resp.books !== null){
        this.books$.next(resp.books);
        this.signalRService.Notify(book);
        var currentOrderSum = 0;
        this.orderSum$.pipe(take(1)).subscribe(x => currentOrderSum = x)
        this.orderSum$.next(currentOrderSum + book.price);
      }
    })
  }

  removeFromBasket(book: Book){
    return this.http.delete(environment.apiUrl + "order/" + book.isbn, {observe: "response"} ).subscribe((resp) => {
      if(resp.status === 200){
        this.books = this.books.filter(bookForDel => book.isbn !== bookForDel.isbn);
        this.books$.next(this.books);
        this.signalRService.Notify(book);
        var currentOrderSum = 0;
        this.orderSum$.pipe(take(1)).subscribe(x => currentOrderSum = x)

        this.orderSum$.next(currentOrderSum < 0 ? 0 : currentOrderSum - book.price);
      }
    })
  }

  placeOrder(){
    var currentOrderSum = 0;
    this.orderSum$.pipe(take(1)).subscribe(x => currentOrderSum = x)
    if(currentOrderSum < 2000)
      return;
    return this.http.post(environment.apiUrl+"order/placeorder", {}, {observe: 'response'})
    .subscribe((response) => 
    {
      this.orderPlaced$.next(response.status === 200); 
      console.log("resp = " + response.status)
    })
    } 
}
