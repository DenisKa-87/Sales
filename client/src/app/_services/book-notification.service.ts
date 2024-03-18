import { Injectable} from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { ItemsService } from './items.service';
import { Book } from '../models/Book';
import { OrderService } from './order.service';
import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class BooksSignalrService {

  hubUrl = environment.apiUrl + "bookhub";
  connection: HubConnection;
  bookToUpdate: Book;

  constructor(private itemsService: ItemsService, private orderService: OrderService) {
  }

  public async initiateSignalRConnection(): Promise<void> {
    try {
      this.connection = new signalR.HubConnectionBuilder().withUrl(this.hubUrl).withAutomaticReconnect().build();

      await this.connection.start();
      console.log(`SignalR connection to bookshub established! connectionId: ${this.connection.connectionId}`);
    }
    catch (error) {
      console.log(`SignalR connection to bookhub failed! Error: ${error}`);
    }

    this.connection.on('updateBook', (book) => {
      this.bookToUpdate = book;
      let index = this.itemsService.books.findIndex(x => x.isbn === book.isbn)
      if (index >= 0)
        this.itemsService.books[index] = book;
      else
        this.itemsService.books.push(book)
      this.itemsService.books$.next(this.itemsService.books);
    });
  }

  public Notify(book: Book) {
    this.connection.invoke("NotifyBookQuantityChange", book.isbn)
  }
}