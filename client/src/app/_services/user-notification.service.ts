import { ComponentFactoryResolver, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { OrderService } from './order.service';
import * as signalR from '@microsoft/signalr';
import { BasketService } from './basket.service';
import { HubConnection } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class UserNotificationService {

  hubUrl = environment.apiUrl + "userhub";
  connection: HubConnection;
  token: string;
  username: string;

  constructor(private orderService: OrderService, private basketService: BasketService) {
  }

  public async initiateSignalRConnection(): Promise<void> {
    try {
      let user = JSON.parse(localStorage.getItem('user'));
      if (user != null) {
        this.token = user.token;
      }
      this.connection = new signalR.HubConnectionBuilder().withUrl(this.hubUrl,
        { accessTokenFactory: () => this.token }
      )
        .withAutomaticReconnect().build();

      await this.connection.start();
      console.log(`SignalR user notification connection success! connectionId: ${this.connection.connectionId}`);
    }
    catch (error) {
      console.log(`SignalR connection error: ${error}`);
    }
    this.connection.on('updateOrder', () => {
        console.log("getCurrent order service")
        this.basketService.getCurrentOrder();
    });
    this.connection.on('updateOrderUrl', (url) => {
      console.log("url from signalr started")
      this.orderService.orderUrl$.next(url);
    })
  }

  public async stopConnection(): Promise<void>{
    try{
      let id = this.connection.connectionId;
      await this.connection.stop();
      console.log(`SignalR user notificatiion connection stopped! connectionId: ${id}`);
    }
    catch(error){
      console.log(`SignalR stopping connection error: ${error}`);
    }
  }

  public SendOrderUpdates(){
    this.connection.invoke("UpdateOrder")
  }
}
