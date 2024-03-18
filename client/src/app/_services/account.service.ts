import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, map } from 'rxjs';
import { User } from '../models/User';
import { environment } from 'src/environments/environment.development';
import { UserNotificationService } from './user-notification.service';

@Injectable({
  providedIn: 'root'
})

export class AccountService {

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient,  private userNotificationService: UserNotificationService) { }

  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user)
  }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
          this.userNotificationService.initiateSignalRConnection();
        }
        return user;
      })
    )
  }

  
  register(){
    return this.http.post(this.baseUrl + 'account/register', {}).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
          this.userNotificationService.initiateSignalRConnection();
        }
        return user;
      })
    )
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.userNotificationService.stopConnection();
  }
}
