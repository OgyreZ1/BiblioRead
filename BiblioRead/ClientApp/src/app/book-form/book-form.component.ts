import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {
  title: string;
  authorName: string;
  year: number;

  onKeyUp() {
    console.log(this.title);
  }


  onSubmit($event) {
    console.log(this.title, this.authorName, this.year);
  }

  constructor() { }

  ngOnInit() {
  }

}
