import { RentalsService } from './../services/rentals.service';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { Rental } from '../models/rental';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {

  constructor(private rentalsService: RentalsService,
              private booksService: BooksService, 
              private userService: UserService, 
              private toastr: ToastrService
              ){ }

  books: Array<Book> = [];
  ngOnInit() {
    this.getBooks();
  }

  getBooks() {
    this.books = [];
    for (let bookId of this.userService.bookIds) {
      this.booksService.getBook(bookId)
      .subscribe(value => (this.books.push(value)))
    }
  }

  onRemove(id: number) {
    let index: number = this.userService.bookIds.indexOf(id);
    this.userService.bookIds.splice(index, 1);
    console.log(this.userService.bookIds);
    
    this.getBooks();
  }

  onSubmit() {
      
    var rental = {
        userId: this.userService.currentUser.id, 
        bookIds: this.userService.bookIds
    }
    this.rentalsService.createRental(rental)
    .subscribe(res => {
        this.toastr.success('You took these books', 'Success!');
        this.userService.bookIds = [];
        this.getBooks();
    },
    err => {
        this.toastr.error(err.value, 'Error')
    }
    );
  }
}

