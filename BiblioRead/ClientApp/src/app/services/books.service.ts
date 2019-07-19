import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../interfaces/book';


const httpOptions = {
  header: new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'my-auth-token'
  })
};

@Injectable()
export class BooksService {
  booksUrl = 'api/books';
  

  constructor(private http: HttpClient) {
  }

  getBooks(): Observable<Book[]>{
    return this.http.get<Book[]>(this.booksUrl);
  }

  getBook(id: number): Observable<Book> {
    return this.http.get<Book>(this.booksUrl + '/' + id);
  }

}
