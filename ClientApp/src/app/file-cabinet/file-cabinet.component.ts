import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { Rental } from '../models/rental';

@Component({
  selector: 'app-file-cabinet',
  templateUrl: './file-cabinet.component.html',
  styles: []
})
export class FileCabinetComponent implements OnInit {

  constructor(private userService: UserService) { }
   users: User[] = [];
   userElements: UserElement[] = [];

   ngOnInit() {
      this.userService.getUsers("Customer").subscribe(
         res => {
            this.users = res;
            var count = 0;
            this.users.forEach(user => {
               var userElement: UserElement = {
                  id: count++,
                  user: user,
                  rentalsShow: false
               };
               this.userElements.push(userElement);
            });
            console.log(this.userElements);
            
         },
         err => {}
      ) 
   }

   onShow(id: number) {
      var element = this.userElements.find(ue => ue.id == id);
      element.rentalsShow = !element.rentalsShow;
   }
}

class UserElement {
   id: number;
   user: User;
   rentalsShow: boolean;
}
