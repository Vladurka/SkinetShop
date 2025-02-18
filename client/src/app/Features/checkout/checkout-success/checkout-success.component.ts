import { Component, inject, OnInit } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../../../Core/services/order.service';
import { Order } from '../../../Shared/models/Order';
import { AddressPipe } from "../../../Shared/pipes/address.pipe";
import { PaymentPipe } from "../../../Shared/pipes/payment.pipe";
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-checkout-success',
  imports: [
    MatButton,
    RouterLink,
    AddressPipe,
    PaymentPipe,
    DatePipe,
    CurrencyPipe
],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent implements OnInit {
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);
  order?: Order;

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder(){
    const id = this.activatedRoute.snapshot.paramMap.get("id");
    if(!id) return;
    this.orderService.getOrderDetailed(id).subscribe({
      next: order => this.order = order
    });
  }
}
