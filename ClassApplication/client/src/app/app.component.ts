import { HttpClient } from '@angular/common/http';
import { typeWithParameters } from '@angular/compiler/src/render3/util';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'The Class App';
  users: any;

  constructor(private http: HttpClient) {}
  ngOnInit(): void {
    this.GetUsers();
  }

  GetUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('complete');
      },
    });
  }
}
