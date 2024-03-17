import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../models/User';
import { Observable } from 'rxjs';
import { BasketService } from '../_services/basket.service';
import { ItemsService } from '../_services/items.service';
import { Book } from '../models/Book';
import { BooksSignalrService } from '../_services/signalr.service';
import { OrderService } from '../_services/order.service';
import { UserNotificationService } from '../_services/user-notification.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent  implements OnInit{

  model: any = {};

  constructor(public accountService: AccountService, private toastr: ToastrService, private basketService: BasketService, 
    private booksService: BooksSignalrService, private orderService: OrderService){}
  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe({next: () => {
      this.toastr.success("Successfully signed in.");
      this.basketService.getCurrentOrder();
    }})
  }

  logout(){
    this.basketService.books$.next([])
    this.accountService.logout();
    this.basketService.orderSum$.next(0);
    this.basketService.orderPlaced$.next(false);
    this.orderService.orderUrl$.next(null);
  }

  register(){
    this.accountService.register().subscribe({next: () => {
      this.toastr.success("Successfully signed in.");
    }})
  }

}
