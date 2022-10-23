import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { UserRole } from './user-role';
import { ConfirmPasswordStateMatcher } from '../confirm-password-state-matcher';
import { confirmPasswordValidator } from '../confirm-password-validator';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  role: string | undefined
  userRoleFormData: UserRole = new UserRole();

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Passwords: this.fb.group ({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, {validators: confirmPasswordValidator('password', 'confirmPassword')})

  });

  confirmPasswordStateMatcher = new ConfirmPasswordStateMatcher();
  
  login(formData: any) {
    return this.http.post('/api/Authentication/login', formData);
  }

  changeUserRole() {
    return this.http.post('/api/Users/set-user-role', this.userRoleFormData);
  }

  removeUser(userId: number) {
    return this.http.delete(`/api/Users/${userId}`);
  }

  defineRole(){
    localStorage.getItem('token') !== null;
    if (localStorage.getItem('token') !== null) {
      if(this.roleMatch(['Admin'])) this.role = 'Admin'
      if(this.roleMatch(['Moderator'])) this.role = 'Moderator'
      if(this.roleMatch(['User'])) this.role = 'User'
    }
    console.log(this.role)
  }

  roleMatch(allowedRoles: any[]): boolean {
    var isMatch = false;
    var payLoad = JSON.parse(window.atob(localStorage.getItem('token')?.split('.')[1] || ''));
    var userRole = payLoad["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    // @ts-ignore
    allowedRoles.forEach(element => {
      if (userRole == element) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }
}
