import { Router } from '@angular/router';
import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html'
})
export class RegistrationComponent implements OnInit {

  constructor(private router: Router,private service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    if (localStorage.getItem('token')  != null)
      this.router.navigateByUrl('/home');
  }

  onSubmit(){
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('You have successfully registred', 'Success!');
          this.router.navigateByUrl('/home');
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
