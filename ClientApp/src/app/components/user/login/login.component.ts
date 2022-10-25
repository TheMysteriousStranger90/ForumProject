import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  formModel = {
    Email: '',
    Password: ''
  }
  constructor(private service: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnDestroy(): void {
    this.service.defineRole();
  }

  ngOnInit() {
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/');
  }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/');
      },
      err => {
        if (err.status == 400){
          this.toastr.error(err.error, 'Authentication failed.');
        }
        else
          console.log(err);
      });
  }
}
