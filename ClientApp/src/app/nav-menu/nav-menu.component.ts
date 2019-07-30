import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html'
})
export class NavMenuComponent implements OnInit{

  ngOnInit() {
  }

  constructor(private service: UserService, private router: Router) {      
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['user/login']);

  }
}
