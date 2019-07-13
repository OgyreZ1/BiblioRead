import { Component, OnInit } from '@angular/core';
import { BookService } from "../services/book.service";

@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {

  constructor(private bookService: BookService) { }

  ngOnInit() {
  }

}
