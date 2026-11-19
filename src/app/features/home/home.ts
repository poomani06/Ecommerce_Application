import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  imports: [RouterLink],
  selector: 'app-home',
  styleUrl: './home.css',
  templateUrl: './home.html',
})
export class Home implements OnInit {

  successMessage = '';

  ngOnInit(): void {
    this.successMessage = localStorage.getItem('authMessage') || '';

    localStorage.removeItem('authMessage');
  }
}
