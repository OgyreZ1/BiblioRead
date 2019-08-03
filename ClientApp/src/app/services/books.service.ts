import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book';
import { catchError } from 'rxjs/operators';


const httpOptions = {
  header: new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'my-auth-token'
  })
};

@Injectable()
export class BooksService {
  booksUrl = 'http://localhost:5000/api/books';

  constructor(private http: HttpClient) {
  }

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.booksUrl);
  }

  getBook(id: number): Observable<Book>{
    return this.http.get<Book>(this.booksUrl + '/' + id);
  }

  createBook(book: Book): Observable<Book> {
    return this.http.post<Book>(this.booksUrl, book);
  }

  deleteBook(id: number) {
    const url = `${this.booksUrl}/${id}`;
    return this.http.delete(url);
  }

  updateBook(id: number, book: Book) {
    console.log(id);
    const url = `${this.booksUrl}/${id}`;
    return this.http.put(url, book);
  }
}

