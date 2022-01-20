import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter<boolean>();
  constructor(private accountService: AccountService) {}

  ngOnInit(): void {}
  register() {
    this.accountService.register(this.model).subscribe({
      next: (data) => {
        console.log(data);
        this.cancel();
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('completed');
      },
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
