import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from "@angular/common/http";

@Injectable()
export class UserService {

  constructor(private fb: FormBuilder, private http:HttpClient) { }
  readonly usersUrl = 'http://localhost:5000/api/ApplicationUsers/'

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FullName: [''],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, {validator: this.comparePasswords })
  });

  comparePasswords(fb: FormGroup){
    let confirmPassword = fb.get('ConfirmPassword');

    if (confirmPassword.errors == null || 'passwordMismatch' in confirmPassword.errors) {
      if (fb.get('Password').value != confirmPassword.value)
        confirmPassword.setErrors({passwordMismatch: true});
      else 
        confirmPassword.setErrors(null);
    }
  }

  register() {
    var fm = this.formModel.value;
    var body = {
      UserName: fm.UserName,
      Email: fm.Email,
      FullName: fm.FullName,
      Password: fm.Passwords.Password,
    }

    return this.http.post(this.usersUrl+'Register', body);
  }
}
