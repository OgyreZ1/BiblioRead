import { Component, OnInit } from '@angular/core';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { UserService } from '../services/user.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-books',
  templateUrl: './books.component.html'
})
export class BooksComponent implements OnInit {
  books: Book[];
  constructor(private toastr: ToastrService, private userService: UserService, private booksService: BooksService) { }

  ngOnInit() {
    this.getBooks();
  }

  getBooks(): void {
    this.booksService.getBooks()
      .subscribe(books => (this.books = books));
  }

  delete(id: number) {
    this.booksService.deleteBook(id)
      .subscribe(() => {
        this.getBooks();
      });
  }

  addToCart(id: number) {
    if (!this.userService.bookIds.includes(id)) {
      this.userService.bookIds.push(id);
      this.toastr.success('You successfully took this book', 'Success');
    }
    else {
      this.toastr.error('You have already took this book', "Error")
    }
    
  }

}
