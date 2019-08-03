import { Injectable, OnInit } from '@angular/core';
import { FormBuilder} from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { User } from '../models/user';
import { Observable } from 'rxjs';

@Injectable()
export class UserService implements OnInit {

  ngOnInit() {
    if (this.authenticated())
      this.loadCurrentUser();
  }

  constructor(private fb: FormBuilder, private http:HttpClient) { }
  readonly baseUrl = 'http://localhost:5000/api'
  public currentUser;
  public bookIds: Array<number> = [1, 2, 3];

  getUserProfile() {
    return this.http.get(this.baseUrl + '/UserProfile');
  }

  getUsers(role: string): Observable<User[]>{
    return this.http.get<User[]>(this.baseUrl + '/ApplicationUsers/' + role);
  }

  deleteUser(id: string) {
    return this.http.delete(this.baseUrl + '/ApplicationUsers/' + id);
  }

  roleMatch(allowedRoles): boolean {
    var isMatch = false;
    var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    var userRole = payLoad.role;
    let roles = allowedRoles.split(', ');
    
    roles.forEach(element => {
      if (userRole == element) {
        isMatch = true
        return false
        ;}
    });
    return isMatch;
  }

  updateUser(user: User) {
    return this.http.put(this.baseUrl + '/UserProfile/', user);
  }

  authenticated() {
    if (localStorage.getItem('token') != null)
      return true;
    return false;
  }

  loadCurrentUser() {
    this.getUserProfile().subscribe(
      res => {
        this.currentUser = res;
      },
      err => console.log(err)
    );
  }
}
