import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmPasswordStateMatcher } from 'src/app/shared/confirm-password-state-matcher';
import { confirmPasswordValidator } from 'src/app/shared/confirm-password-validator';
import { UserService } from 'src/app/shared/user/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  formModelreg = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Password: [''],
    ConfirmPassword: ['']
  }, {validators: confirmPasswordValidator('Password', 'ConfirmPassword')});

  confirmPasswordStateMatcher = new ConfirmPasswordStateMatcher();

  hidePassword = true;
  hideConfirmPassword = true;
  inProgress = false;


  constructor(public service: UserService, private router: Router, private toastr: ToastrService, private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit() {
  }

  onSubmit() {
    if (this.formModelreg.invalid) {
      return;
    }

    this.inProgress = true;

    var body = {
      UserName: this.formModelreg.value.UserName,
      FirstName: this.formModelreg.value.FirstName,
      LastName: this.formModelreg.value.LastName,
      Email: this.formModelreg.value.Email,
      Password: this.formModelreg.value.Password
    };


    return this.http.post('/api/Authentication/registration', body).subscribe(
      (res: any) => {
        this.service.formModel.reset();
        this.toastr.success('New user created! Now you can try to log in', 'Registration successful.');
        this.router.navigateByUrl('/user/login');
      },
      err => {
        if (err.status == 400) {
          this.toastr.error(err.error, 'Authentication failed.');
          this.inProgress = false;
        } else
          console.log(err);
      });
  }
}
