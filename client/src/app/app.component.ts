import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./Layouts/Header/Header.component";
import { HttpClient } from '@angular/common/http';
import { Product } from './Shared/models/Product';
import { Pagination } from './Shared/models/Pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  baseUrl='https://localhost:5001/api/'
  constructor(private http: HttpClient) {}
  title = 'Skinet';
  products: Product[] = [];

  ngOnInit(): void {
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: response => this.products = response.data,
      error: err => console.log(err),
      complete: () => console.log('complete')
    });    
  }
}
