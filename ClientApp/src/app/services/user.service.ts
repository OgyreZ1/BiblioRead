import { Injectable, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable()
export class UserService implements OnInit {

  ngOnInit() {
    if (this.authenticated())
      this.loadCurrentUser();
  }

  constructor(private fb: FormBuilder, private http:HttpClient) { }
  readonly usersUrl = 'http://localhost:5000/api'
  public currentUser;

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

  register(role: string = "Customer") {
    var fm = this.formModel.value;
    var body = {
      UserName: fm.UserName,
      Email: fm.Email,
      FullName: fm.FullName,
      Password: fm.Passwords.Password,
      Role: role
    }
    return this.http.post(this.usersUrl+'/ApplicationUsers/Register', body);
  }

  login(formData) {
    return this.http.post(this.usersUrl+'/ApplicationUsers/Login', formData);
  }

  logout() {
    localStorage.removeItem('token');
  }

  getUserProfile() {
    return this.http.get(this.usersUrl + '/UserProfile');
  }

  loadCurrentUser() {
    this.getUserProfile().subscribe(
      res => {
        this.currentUser = res;
      },
      err => console.log(err)
    );
  }
  
  authenticated() {
    if (localStorage.getItem('token') != null)
      return true;
    return false;
  }

  roleMatch(allowedRoles): boolean {
    var isMatch = false;
    var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    var userRole = payLoad.role;
    let roles = allowedRoles.split(', ');
    
    roles.forEach(element => {
      if (userRole == element) {
        isMatch = true
        return false
        ;}
    });
    return isMatch;
  }
}
