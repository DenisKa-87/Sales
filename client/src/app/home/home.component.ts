import { Component, OnInit } from '@angular/core';
import { BasketService } from '../_services/basket.service';
import { AccountService } from '../_services/account.service';
import { OrderService } from '../_services/order.service';
import { UserNotificationService } from '../_services/user-notification.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent{


  orderSum = 0;
  orderPlaced = false;
  url: string = null;

  constructor(private basketService: BasketService, public accountService: AccountService, private orderService: OrderService,
     private userNotificationService: UserNotificationService){
    this.basketService.orderSum$.subscribe(Sum => {
      this.orderSum = Sum;
    }) 
    this.basketService.orderPlaced$.subscribe(placed => {
      this.orderPlaced = placed;
    })
    this.orderService.orderUrl$.subscribe(url => {
      this.url = url;
    })
  }

  placeOrder(){
    this.basketService.placeOrder();
    this.userNotificationService.SendOrderUpdates();
  }
}
