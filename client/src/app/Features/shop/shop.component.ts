import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../Core/Services/shop.service';
import { Product } from '../../Shared/models/Product';
import { MatCard, MatCardContent } from '@angular/material/card';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-shop',
  imports: [
    MatCard,
    ProductItemComponent,
    MatButton,
    MatIcon
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  private shopService = inject(ShopService);  
  private dialogService = inject(MatDialog);  
  products: Product[] = [];

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop(){
    this.shopService.getTypes();
    this.shopService.getBrands();
    this.shopService.getProducts().subscribe({
      next: response => this.products = response.data,
      error: err => console.log(err),
    });    
  }

  openFiltersDialog(){
    const dialogRef = this.dialogService.open(FiltersDialogComponent,{
      minWidth: "500px"
    });
  }
}
