import { Component, inject, OnInit } from '@angular/core';
import { OrderService } from '../../Core/services/order.service';
import { Order } from '../../Shared/models/Order';
import { RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-order',
  imports: [
    RouterLink,
    CurrencyPipe,
    DatePipe
  ],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent implements OnInit{
  private orderService = inject(OrderService);
  orders: Order[] = [];

  ngOnInit() : void{
    this.orderService.getOrdersForUser().subscribe({
      next: orders => this.orders = orders
    });
  }
}
