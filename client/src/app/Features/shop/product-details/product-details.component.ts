import { Component, inject } from '@angular/core';
import { ShopService } from '../../../Core/Services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../Shared/models/Product';
import { CurrencyPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-product-details',
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent {
   private shopService = inject(ShopService);
   private activatedRoute = inject(ActivatedRoute);
   product?: Product;

   ngOnInit() : void{
      this.loadProduct();
   }

   loadProduct(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(!id) return;
    this.shopService.getProduct(id).subscribe({
      next: product => this.product = product,
      error: error => console.log(error)
    });
   }
}
