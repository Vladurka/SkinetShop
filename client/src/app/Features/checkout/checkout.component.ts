import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { OrderSummaryComponent } from "../../Shared/components/order-summary/order-summary.component";
import { Address } from '../../Shared/models/user';
import {MatStepperModule} from "@angular/material/stepper"
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { StripeService } from '../../Core/services/stripe.service';
import { StripeAddressElement, StripePaymentElement } from '@stripe/stripe-js';
import { SnackbarService } from '../../Core/services/snackbar.service';
import {MatCheckbox, MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../Core/services/account.service';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";

@Component({
  selector: 'app-checkout',
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    MatButton,
    RouterLink,
    MatCheckbox,
    CheckoutDeliveryComponent
],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit, OnDestroy{
  private stripedService = inject(StripeService);
  private snackbar = inject(SnackbarService);
  private accountService = inject(AccountService);
  addressElement?: StripeAddressElement;
  paymentElement?: StripePaymentElement;
  saveAddress = false;

  async ngOnInit(){
    try{
      this.addressElement = await this.stripedService.createAddressElement();
      this.addressElement.mount("#address-element");

      this.paymentElement = await this.stripedService.createPaymentElement();
      this.paymentElement.mount("#payment-element");
    }
    catch(error:any){
      this.snackbar.error(error.message);
    }
  }

  async onStepChanged(event: StepperSelectionEvent){
    if(event.selectedIndex === 1){
      if(this.saveAddress){
        const address = await this.getAddressFromStripeAddress();
        address && firstValueFrom(this.accountService.updateAddress(address)); 
      }
    }
    if(event.selectedIndex === 2){
      await firstValueFrom(this.stripedService.createOrUpdatePaymentEvent());
    }
  }

  private async getAddressFromStripeAddress() : Promise<Address | null>{
    const result = await this.addressElement?.getValue();
    const address = result?.value.address;

    if(address){
        return{
          line1: address.line1,
          line2: address.line2 ?? "",
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
