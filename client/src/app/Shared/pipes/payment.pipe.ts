import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

@Pipe({
  name: 'payment'
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken["payment_method_preview"], ...args: unknown[]): unknown {
    if (value && 'card' in value) {
      const {brand, last4, exp_month, exp_year} = 
        (value as ConfirmationToken['payment_method_preview']).card!
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${exp_month}/${exp_year}`;
    } 
    else {
      return 'Unknown payment method'
    }
  }

}
