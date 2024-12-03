import { Component, inject } from '@angular/core';
import { ShopService } from '../../../Core/Services/shop.service';

@Component({
  selector: 'app-filters-dialog',
  imports: [],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  shopService = inject(ShopService);
}
