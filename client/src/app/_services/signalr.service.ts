import { Injectable, Signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environments/environment.development';
import { ItemsService } from './items.service';
import { Book } from '../models/Book';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {


  hubUrl = environment.apiUrl + "demohub";
  connection: HubConnection;
  bookToUpdate: Book;
  constructor(
    private itemsService: ItemsService
    ) {
    
   }

  public async initiateSignalRConnection() :Promise<void> {
    try{
      this.connection = new signalR.HubConnectionBuilder().withUrl(this.hubUrl).withAutomaticReconnect().build();
      await this.connection.start();
      console.log(`SignalR connection success! connectionId: ${this.connection.connectionId}`);
    }
    catch (error) {
      console.log(`SignalR connection error: ${error}`);
    }

    this.connection.on('Hello', (message) => {
      console.log(message)
    })

    this.connection.on('updateBook', (book) => {
      this.bookToUpdate = book;
      let index = this.itemsService.books.findIndex(x => x.isbn === book.isbn)
      if(index >= 0)
        this.itemsService.books[index] = book;
      else
        this.itemsService.books.push(book)
      this.itemsService.books$.next(this.itemsService.books);
    })

  }


   public Notify(book: Book){
     this.connection.invoke("NotifyBookQuantityChange", book.isbn)
   }

  //  public cancelBookReservation(book: Book){
  //   this.connection.invoke("CancelBookReservation", book.isbn);
  //  }
 
}
