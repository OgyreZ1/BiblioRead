import { Component, OnInit } from '@angular/core';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { Rental } from '../models/rental';
import { RentalsService } from '../services/rentals.service';
import { UserService } from '../services/user.service';
import { User } from '../models/user';


@Component({
  selector: 'app-user-books',
  templateUrl: './user-rentals.component.html',
  styles: []
})
export class UserRentalsComponent implements OnInit {

    constructor(private userService: UserService, private rentalsService: RentalsService, private booksService: BooksService) { }
    

    ngOnInit() {
        if (this.userService.authenticated()){
            this.rentalsService.getRentals();     
        }
    } 

    

    onFinish(id: number) {
        this.rentalsService.finishRental(id).subscribe(res => {
            this.userService.loadCurrentUser();
            this.rentalsService.getRentals();
        }, err => {});
        
    }
}
