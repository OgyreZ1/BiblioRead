import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html'
})
export class NavMenuComponent implements OnInit{
  currentUser;
  ngOnInit() {
    if (this.userService.authenticated())
      this.userService.loadCurrentUser();
  }

  constructor(private userService: UserService, private authService: AuthenticationService, private router: Router) {  

  }

  get isAdmin() {
    return false;
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['user/login']);

  }
}
