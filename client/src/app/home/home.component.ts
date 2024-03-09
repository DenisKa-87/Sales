import { Component, OnInit } from '@angular/core';
import { BasketService } from '../_services/basket.service';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{

  constructor(public basketService: BasketService, public accountService: AccountService){}
  
  orderSum = 0;
  ngOnInit(): void {
    this.basketService.orderSum$.subscribe(Sum => {
      this.orderSum = Sum;
    }) 
  }

  placeOrder(){
    this.basketService.placeOrder();
  }
}
