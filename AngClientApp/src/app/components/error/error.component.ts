import { Component, OnInit } from '@angular/core';
import { ElementRef, ViewChild } from '@angular/core';
import { ERROR_INFO } from '../../services/auth.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {

  constructor() { }

  DisplayMessage(): string | null {
    return localStorage.getItem(ERROR_INFO);
  }

}
