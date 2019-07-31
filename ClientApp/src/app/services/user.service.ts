import { Injectable, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthenticationService } from './authentication.service';

@Injectable()
export class UserService implements OnInit {

  ngOnInit() {
    if (this.authenticated())
      this.loadCurrentUser();
  }

  constructor(private fb: FormBuilder, private http:HttpClient) { }
  readonly usersUrl = 'http://localhost:5000/api'
  public currentUser;

  getUserProfile() {
    return this.http.get(this.usersUrl + '/UserProfile');
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
