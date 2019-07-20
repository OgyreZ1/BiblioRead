import { Component, OnInit } from '@angular/core';
import { Book } from '../interfaces/book';
import { BooksService } from '../services/books.service';


@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {
  book: Book = {
    title: '',
    authorName: ''
  };
  receivedBook: Book = new Book();
  fail: boolean = false;
  success: boolean = false;
  error;

  constructor(private bookService: BooksService) { }

  ngOnInit() {

  }


  onSubmit() {
    //console.log(this.book);
    this.bookService.createBook(this.book)
      .subscribe(
      (data: Book) => {
          this.receivedBook = data;
          this.fail = false;
          this.success = true;
        },
        error => {
          this.error = error;
        console.log(error);
        this.fail = true;
      }
    );;
  }

  


}
