import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { first } from "rxjs/operators";
import { AuthorizationService } from "../../../shared/services/authorization.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = "";
  date: Date;
  chngePassword = false;
  pwdPattern = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{12,}/;

  constructor(
    private _formBuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    private _authenticationService: AuthorizationService
  ) {
    this.date = new Date();
  }

  ngOnInit() {
    this._authenticationService.currentUser.next(null);
    this._authenticationService.isAuthenticated.next(false);

    this.loginForm = this._formBuilder.group({
      username: ["", Validators.required],
      password: ["", Validators.required],
      repeatPassword: [""],
    });

    this.returnUrl = this._route.snapshot.queryParams["returnUrl"] || "/";
  }

  get f() {
    return this.loginForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    console.log(this.f.repeatPassword.errors);
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this._authenticationService
      .login(this.f.username.value, this.f.password.value, this.f.repeatPassword.value)
      .pipe(first())
      .subscribe(
        (data) => {
          if(data == null){
            this.error = "Nieudana próba logowania";
            this.loading = false;
            return;
          } else if(data.status == 1) {
            this.error = "Wymagana zmiana hasła";
            this.f.password.setValue("");
            this.f.password.setValidators([Validators.required, Validators.minLength(12), Validators.maxLength(22), Validators.pattern(this.pwdPattern)]);
            this.f.repeatPassword.setValidators([Validators.required, Validators.maxLength(22), this.createCompareValidator(this.f.password, this.f.repeatPassword)]);
            this.f.password.reset();
            this.f.repeatPassword.reset();
            this.f.password.markAsUntouched();
            this.f.repeatPassword.markAsUntouched();
            this.chngePassword = true;
          } else if(data.status == 2) {
            this._router.navigate([this.returnUrl]);
          } else if(data.status == 3) {
            this.error = "Konta zablokowane, zgłoś się do aministratora.";
          } else {
            this.error = "Nieudana próba logowania, zgłoś się do aministratora.";
          }
          this.loading = false;
        },
        (error) => {
          this.error = "Nieudana próba logowania";
          this.loading = false;
        }
      );
    }
    createCompareValidator(controlOne: AbstractControl, controlTwo: AbstractControl) {
      return () => {
        if (controlOne.value !== controlTwo.value)
          return { match_error: 'Value does not match' };
        return null;
      };
    }

    cancelChangePass() {
      this.f.password.setValidators([Validators.required]);
      this.f.repeatPassword.setValidators(null);
      this.f.password.reset();
      this.f.repeatPassword.reset();
      this.f.password.markAsUntouched();
      this.f.repeatPassword.markAsUntouched();
      this.chngePassword = false;
    }
  }
