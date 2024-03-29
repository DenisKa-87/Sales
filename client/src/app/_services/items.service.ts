import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject} from 'rxjs';
import { Book } from '../models/Book';
import { environment } from 'src/environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class ItemsService {

  books$ = new BehaviorSubject<Book[]>([]);
  books: Book[];


  constructor(private http: HttpClient) {
    this.books = []
  }

  getItems() {
    return this.http.get<Book[]>(environment.apiUrl + 'Books').subscribe(resp => {
      this.books$.next(resp);
      this.books = resp;
    });;
  }
}
