import { AuthenticationService } from './../../services/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
formModel = {
  UserName: '',
  Password: ''
}

  constructor(private authService:AuthenticationService,
              private userService: UserService,
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit() {
    if (localStorage.getItem('token')  != null)
      this.router.navigateByUrl('/home');
  }

  onSubmit(form: NgForm) {
    this.authService.login(form.value).subscribe(
      (res:any)=>{
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/home');
        this.userService.loadCurrentUser();
      },
      err => {
        if (err.status == 400) 
          this.toastr.error('Incorrect username or password', 'Authentication failed');
        else 
          console.log(err);
      }
    );
    
  }
}
