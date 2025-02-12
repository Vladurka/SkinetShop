import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart } from '../../Shared/models/Cart';
import { Product } from '../../Shared/models/Product';
import { map } from 'rxjs';
import { v4 as uuidv4 } from 'uuid';


@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantityInStock, 0)
  })

  itemsPrice = computed(() => {
    const cart = this.cart();
    if(!cart) return null;
    const subtotal = cart.items.reduce((sum, item) => sum + item.price * item.quantityInStock, 0)
    const discount = 25;
    const shipping = 15;
    return{
      subtotal,
      shipping,
      discount,
      total: subtotal - (subtotal * discount / 100) + shipping
    }
  })

  getCart(cartId: string){
    return this.http.get<Cart>(`${this.baseUrl}cart/${cartId}`).pipe(
      map(cart =>{
        this.cart.set(cart);
        return cart;
      })
    )
  }

  setCart(cart: Cart) {
    console.log('Setting cart:', cart);
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: (responseCart) => {
        console.log('Cart set successfully:', responseCart);
        this.cart.set(responseCart);
      },
      error: (err) => console.error('Failed to set cart:', err),
    });
  }  

  addItemToCart(item: Product, quantity = 1) {
    let cart = this.cart();
  
    if (!cart) {
      console.log('Cart not found. Creating a new one.');
      cart = this.createCart();
    }
  
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    this.setCart(cart);
    this.cart.set(cart);
  }  

  removeItemFromCart(productId: string, quantity = 1){
    const cart = this.cart();
    if(!cart) return;
    const index = cart.items.findIndex((x) => x.id === productId);
    if(index !== -1){
      if(cart.items[index].quantityInStock > 0){
        cart.items[index].quantityInStock -= quantity;
      }
      else{
        cart.items.splice(index, 1);
      }
      if(cart.items.length === 0){
        this.deleteCart();
      }
      else{
        this.setCart(cart);
      }
    }
  }

  deleteCart() {
   this.http.delete(this.baseUrl + 'cart?id=' + this.cart()?.id).subscribe({
    next: () =>{
      localStorage.removeItem('cart_id')
      this.cart.set(null);
    }
   })
  }

  addOrUpdateItem(items: Product[], item: Product, quantity: number): Product[] {
    const index = items.findIndex((x) => x.id === item.id);

    if (index === -1) {
      items.push({ ...item, quantityInStock: quantity });
    } else {
      items[index] = {
        ...items[index],
        quantityInStock: items[index].quantityInStock + quantity,
      };
    }

    return items;
  }

  private createCart(): Cart {
    const cart = new Cart();
    cart.id = uuidv4();
    localStorage.setItem('cart_id', cart.id);
    this.cart.set(cart);
    return cart;
  }
}

