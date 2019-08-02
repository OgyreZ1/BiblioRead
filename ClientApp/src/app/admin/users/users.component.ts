import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  users: User[];
  title: string = 'Customers';
  role: string;
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getUsers("Customer");
  }

  getUsers(role: string): void {
    this.userService.getUsers(role)
    .subscribe(customers => this.users = customers);
    this.role = role;
    this.title = role + 's';
  }

  delete(id: string) {
    this.userService.deleteUser(id)
      .subscribe(() => {
        this.getUsers(this.role);
      })
  }

  promote(user: User) {
    user.role = "Librarian";
    this.userService.updateUser(user)
    .subscribe(() => {
      this.getUsers(this.role);
    });
  }

  demote(user: User) {
    user.role = "Customer";
    this.userService.updateUser( user)
    .subscribe(() => {
      this.getUsers(this.role);
    });
  }


}
