import { Component, OnInit } from '@angular/core';
import { Book } from '../classes/book';
import { BooksService } from '../services/books.service';


@Component({
  selector: 'app-books',
  templateUrl: './books.component.html'
})
export class BooksComponent implements OnInit {
  books: Book[];
  constructor(private booksService: BooksService) { }

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
