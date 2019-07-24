import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html'
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }

  onSubmit(){
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('You have successfully registred', 'Success!');
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
