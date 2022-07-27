import { Component, OnInit } from '@angular/core';
import { SUCCESS_INFO } from '../../services/auth.service';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.css']
})

export class SuccessComponent {

  constructor() { }

  DisplayMessage(): string | null {
    return localStorage.getItem(SUCCESS_INFO);
  }

}
