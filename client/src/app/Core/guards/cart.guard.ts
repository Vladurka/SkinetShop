import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import {inject} from '@angular/core'
import { SnackbarService } from '../services/snackbar.service';

export const cartGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const router = inject(Router);
  const snack = inject(SnackbarService)

  if(!cartService.cart || cartService.cart()?.items.length === 0){
    router.navigate(["/cart"], {queryParams: {returnUrl: state.url}});
    snack.error("The cart is empty");
    return false;
  }
  return true
}