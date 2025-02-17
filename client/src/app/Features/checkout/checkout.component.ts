import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { OrderSummaryComponent } from "../../Shared/components/order-summary/order-summary.component";
import { Address } from '../../Shared/models/user';
import {MatStepper, MatStepperModule} from "@angular/material/stepper"
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { StripeService } from '../../Core/services/stripe.service';
import { ConfirmationToken, StripeAddressElement, StripeAddressElementChangeEvent, StripePaymentElement, StripePaymentElementChangeEvent } from '@stripe/stripe-js';
import { SnackbarService } from '../../Core/services/snackbar.service';
import {MatCheckbox, MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../Core/services/account.service';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";
import { CheckoutReviewComponent } from "./checkout-review/checkout-review.component";
import { CartService } from '../../Core/services/cart.service';
import { CurrencyPipe} from '@angular/common';
import {MatProgressSpinner, MatProgressSpinnerModule} from '@angular/material/progress-spinner'
import { OrderToCreate, ShippingAddress } from '../../Shared/models/Order';
import { OrderService } from '../../Core/services/order.service';

@Component({
  selector: 'app-checkout',
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink,
    MatCheckbox,
    CheckoutDeliveryComponent,
    CheckoutReviewComponent,
    CurrencyPipe,
    MatProgressSpinnerModule
],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit, OnDestroy{
  private stripedService = inject(StripeService);
  private snackbar = inject(SnackbarService);
  private accountService = inject(AccountService);
  private orderService = inject(OrderService);
  private router = inject(Router);
  cartService = inject(CartService);
  addressElement?: StripeAddressElement;
  paymentElement?: StripePaymentElement;
  saveAddress = false;
  completionStatus = signal<{address: boolean, card: boolean, delivery: boolean}>({address: false, card: false, delivery: false});

  confirmationToken?: ConfirmationToken;
  loading = true;

  constructor(){
    this.handleAddressChange = this.handleAddressChange.bind(this);
  }

  async ngOnInit(){
    try{
      this.addressElement = await this.stripedService.createAddressElement();
      this.addressElement.mount("#address-element");
      this.addressElement.on("change", this.handleAddressChange);

      this.paymentElement = await this.stripedService.createPaymentElement();
      this.paymentElement.mount("#payment-element");
      this.paymentElement.on("change", this.handlePaymentChange);
    }
    catch(error:any){
      this.snackbar.error(error.message);
    }
  }

  handleAddressChange = (event: StripeAddressElementChangeEvent) => {
    this.completionStatus.update(state => {
      state.address = event.complete;
      return state;
    });
  }

  handlePaymentChange = (event: StripePaymentElementChangeEvent) => {
    this.completionStatus.update(state => {
      state.card = event.complete;
      return state;
    });
  }

  handleDeliveryChange (event: boolean) {
    this.completionStatus.update(state => {
      state.delivery = event;
      return state;
    });
  }

  async getConfirmationToken(){
    this.loading = true;
    try{
      if(Object.values(this.completionStatus()).every(status => status === true)){
        const result = await this.stripedService.createConfirmationToken();
        if(result.error) throw new Error(result.error.message);
        this.confirmationToken = result.confirmationToken;
        console.log(this.confirmationToken);
      }
    }
    catch(error: any){
      this.snackbar.error(error.message);
    }
    finally{
      this.loading = false;
    }
  }

  async onStepChanged(event: StepperSelectionEvent){
    if(event.selectedIndex === 1){
      if(this.saveAddress){
        const address = await this.getAddressFromStripeAddress() as Address;
        address && firstValueFrom(this.accountService.updateAddress(address)); 
      }
    }
    if(event.selectedIndex === 2){
      await firstValueFrom(this.stripedService.createOrUpdatePaymentIntent());
    }
    if(event.selectedIndex === 3){
      await this.getConfirmationToken();
    }
  }

  async confirmPayment(stepper: MatStepper){
    try{
      if(this.confirmationToken){
        const result = await this.stripedService.confirmPayment(this.confirmationToken);

        if(result.paymentIntent?.status === "succeeded"){
          const order = await this.createOrderModel();    
          const orderResult = await firstValueFrom(this.orderService.createOrder(order)); 
          
          if(orderResult){
            this.cartService.deleteCart();
            this.cartService.selectedDelivery.set(null);
            this.router.navigateByUrl("checkout/success");
          }
          else{
            throw new Error("Order creation failed");
          }
        }
        else if(result.error){
          throw new Error(result.error.message);
        }
        else{
          throw new Error("Something went wrong");
        }
      }
    }
    catch(error: any){
      this.snackbar.error(error.message || "Something went wrong");
      stepper.previous();
    }
  }

  private async createOrderModel(): Promise<OrderToCreate> {
    const cart = this.cartService.cart();
    const shippingAddress = await this.getAddressFromStripeAddress() as ShippingAddress;
    const card = this.confirmationToken?.payment_method_preview.card;

    if (!cart?.id || !cart.deliveryMethodId || !card || !shippingAddress) {
      throw new Error('Problem creating order');
    }

    return {
      cartId: cart.id,
      paymentSummary: {
        last4: +card.last4,
        brand: card.brand,
        expMonth: card.exp_month,
        expYear: card.exp_year
      },
      deliveryMethodId: cart.deliveryMethodId,
      shippingAddress
    }
  }

  private async getAddressFromStripeAddress() : Promise<Address | ShippingAddress | null>{
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;

    if(address){
        return{
          name: result.value.name,
          line1: address.line1,
          line2: address.line2 ?? undefined,
          city: address.city,
          state: address.state,
          country: address.country,
          postalCode: address.postal_code
        }
    }
    else return null;
  }

  
  onSaveAddressCheckbox(event: MatCheckboxChange){
    this.saveAddress = event.checked;
  }

  ngOnDestroy(): void {
    this.stripedService.disposeElements();
  }
}
