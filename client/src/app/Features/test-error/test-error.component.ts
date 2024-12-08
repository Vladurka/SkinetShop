import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  imports: [
    MatButton
  ],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent {
  baseUrl = 'https://localhost:5001/api/'
  private http = inject(HttpClient);
  validationErrors?: string[];

  getError(errorName: string){
    this.http.get(this.baseUrl + 'buggy/' + errorName).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  getValidationError(){
    this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next: response => console.log(response),
      error: error => this.validationErrors = error
    });
  }
}
