import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  users: User[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers(): void {
    this.userService.getUsers()
      .subscribe(users => (this.users = users));
  }

  delete(id: string) {
    this.userService.deleteUser(id)
      .subscribe(() => {
        this.getUsers();
      })
  }

  promote(user: User) {
    user.role = "Librarian";
    this.userService.updateUser(user)
    .subscribe(() => {
      this.getUsers();
    });
  }

  demote(user: User) {
    user.role = "Customer";
    this.userService.updateUser( user)
    .subscribe(() => {
      this.getUsers();
    });
  }


}
