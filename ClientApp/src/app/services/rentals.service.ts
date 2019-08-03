import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rental } from '../models/rental';

@Injectable({
  providedIn: 'root'
})
export class RentalsService {
    private url = 'http://localhost:5000/api/rentals'
    constructor(private http: HttpClient) { }

    public createRental(rental: Rental): Observable<Rental> {
        return this.http.post<Rental>(this.url, rental);
    }
}
