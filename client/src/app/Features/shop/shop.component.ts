import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../Core/Services/shop.service';
import { Product } from '../../Shared/models/Product';
import { MatCard, MatCardContent } from '@angular/material/card';
import { ProductItemComponent } from "./product-item/product-item.component";

@Component({
  selector: 'app-shop',
  imports: [
    MatCard,
    ProductItemComponent
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  private shopService = inject(ShopService);  
  products: Product[] = [];

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop(){
    this.shopService.getBrands();
    this.shopService.getProducts();

    this.shopService.getProducts().subscribe({
      next: response => this.products = response.data,
      error: err => console.log(err),
    });    
  }
}
