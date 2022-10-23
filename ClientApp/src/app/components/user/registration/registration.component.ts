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

  hidePassword = true;
  hideConfirmPassword = true;
  inProgress = false;


  constructor(public service: UserService, private router: Router, private toastr: ToastrService, private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit() {
  }

  onSubmit() {
    if (this.formModel.invalid) {
      return;
    }

    this.inProgress = true;

    var body = {
      UserName: this.formModel.value.UserName,
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Passwords
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
