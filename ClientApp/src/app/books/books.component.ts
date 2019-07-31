import { Component, OnInit } from '@angular/core';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { UserService } from '../services/user.service';


@Component({
  selector: 'app-books',
  templateUrl: './books.component.html'
})
export class BooksComponent implements OnInit {
  books: Book[];
  constructor(private userService: UserService, private booksService: BooksService) { }

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

}
