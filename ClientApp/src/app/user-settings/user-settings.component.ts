import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './user-settings.component.html',
})
export class UserSettingsComponent implements OnInit{
  userDetails;

  ngOnInit() {
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      }
    )
  }

  constructor(private service: UserService) {      
  }

}
