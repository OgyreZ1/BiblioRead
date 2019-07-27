import { Component, OnInit } from '@angular/core';
import { BooksService } from '../services/books.service';
import { Book } from '../classes/book';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  book: Book;
  title;
  id: number;
  private sub: any;
  error;
  fail: boolean = false;
  success: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private booksService: BooksService
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id'];
    });

    this.booksService.getBook(this.id)
      .subscribe(book => (this.book = book));

  }

  onEdit(book: Book) {
    this.success = false;
    this.fail = false;

    this.booksService.updateBook(this.book.id, book)
      .subscribe(
        (data: Book) => {
          this.success = true;
        },
        error => {
          this.fail = true;
          this.error = error;
        }
        );
  }
}