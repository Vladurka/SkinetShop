import { Component, inject, OnInit } from '@angular/core';
import { CheckoutService } from '../../../Core/services/checkout.service';
import {MatRadioModule} from "@angular/material/radio"
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-checkout-delivery',
  imports: [
    MatRadioModule,
    CurrencyPipe
  ],
  templateUrl: './checkout-delivery.component.html',
  styleUrl: './checkout-delivery.component.scss'
})
export class CheckoutDeliveryComponent implements OnInit{
  checkoutService = inject(CheckoutService);

  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods().subscribe();
  }
}


