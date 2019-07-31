import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-librarian',
  templateUrl: './add-librarian.component.html'
})
export class AddLibrarianComponent implements OnInit {

  constructor(private service: UserService, private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
  }

  onSubmit(){
    this.service.register("Librarian").subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('Librarian has successfully registred', 'Success!');
        } else {
          res.errors.forEach(element => {
             switch (element.code) {
               case 'DuplicateUserName':
                 this.toastr.error(element.description, 'Registration failed')
                 break;
             
               default:
                 break;
             }
          });
        }
      },
      error => {
        console.log(error);
      }
    );
  }
}
