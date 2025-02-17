import { Routes } from '@angular/router';
import { HomeComponent } from './Features/home/home.component';
import { ShopComponent } from './Features/shop/shop.component';
import { ProductDetailsComponent } from './Features/shop/product-details/product-details.component';
import { NotFoundComponent } from './Shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './Shared/components/server-error/server-error.component';
import { TestErrorComponent } from './Features/test-error/test-error.component';
import { CartComponent } from './Features/cart/cart.component';
import { CheckoutComponent } from './Features/checkout/checkout.component';
import { LoginComponent } from './Features/account/login/login.component';
import { RegisterComponent } from './Features/account/register/register.component';
import { authGuard } from './Core/guards/auth.guard';
import { cartGuard } from './Core/guards/cart.guard';
import { CheckoutSuccessComponent } from './Features/checkout/checkout-success/checkout-success.component';
import { OrderComponent } from './Features/orders/order.component';
import { OrderDetailedComponent } from './Features/orders/order-detailed/order-detailed.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'shop', component: ShopComponent},
  {path: 'shop/:id', component: ProductDetailsComponent},
  {path: 'cart', component: CartComponent},
  {path: 'checkout', component: CheckoutComponent, canActivate: [authGuard, cartGuard]},
  {path: 'checkout/success', component: CheckoutSuccessComponent, canActivate: [authGuard]},
  {path: 'orders', component: OrderComponent, canActivate: [authGuard]},
  {path: 'order/:id', component: OrderDetailedComponent, canActivate:[authGuard]},
  {path: 'account/login', component: LoginComponent},
  {path: 'account/register', component: RegisterComponent},
  {path: 'test-error', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', redirectTo:'not-found', pathMatch: 'full'}
];
