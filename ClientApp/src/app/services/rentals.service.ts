import { UserService } from 'src/app/services/user.service';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rental } from '../models/rental';

@Injectable({
  providedIn: 'root'
})
export class RentalsService {
    private url = 'http://localhost:5000/api/rentals'
    constructor(private http: HttpClient, private userService: UserService) { }

    rentals: Rental[] = [];
    finishedRentals: Rental[] = [];
    unfinishedRentals: Rental[] = [];

    public createRental(rental: any): Observable<Rental> {
        return this.http.post<Rental>(this.url, rental);
    }

    public getRental(id): Observable<Rental> {
        return this.http.get<Rental>(this.url + '/' + id);
    }

    public finishRental(id) {
        return this.http.put(this.url + '/finish/' + id, {});
    }

    getRentals() {
        this.rentals = [];
        this.finishedRentals = [];
        this.unfinishedRentals = [];
        if (this.userService.authenticated())
            this.userService.getUserProfile().subscribe(
                res => {
                    this.rentals = res.rentals;
                    this.getFinishedRentals();
                }, 
                err => {

                }
            )
    }

    getFinishedRentals() {
        this.rentals.forEach(rental => {
            if (rental.isFinished == true) {
                this.finishedRentals.push(rental);
            } else {
                this.unfinishedRentals.push(rental);
            }
        });
    }
}
