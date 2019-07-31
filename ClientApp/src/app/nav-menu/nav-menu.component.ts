import { Role } from './../models/role';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html'
})
export class NavMenuComponent implements OnInit{
  currentUser;
  ngOnInit() {
    if (this.service.authenticated())
      this.service.loadCurrentUser();
  }

  constructor(private service: UserService, private router: Router) {  

  }

  get isAdmin() {
    return false;
  }

  onLogout() {
    this.service.logout();
    this.router.navigate(['user/login']);

  }
}
