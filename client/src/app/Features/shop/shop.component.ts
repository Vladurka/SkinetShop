import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../Core/services/shop.service';
import { Product } from '../../Shared/models/Product';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../Shared/models/ShopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../Shared/models/Pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  private shopService = inject(ShopService);  
  private dialogService = inject(MatDialog);  
  products?: Pagination<Product>;
  sortOptions=[
    {name: 'Alphabetical', value:'Name'},
    {name: 'Price: Low-High', value:'priceAsc'},
    {name: 'Price: High-low', value:'priceDesc'}
  ]

  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 15, 20]

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop(){
    this.shopService.getTypes();
    this.shopService.getBrands();
    this.getProducts();
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.products = response,
      error: err => console.log(err)
    });     
  }

  onSearchChanged(){
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  handlePageEvent(event: PageEvent){
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  onSortChange(event: MatSelectionListChange){
    const selectedOptions = event.options[0];
    if(selectedOptions){
      this.shopParams.sort = selectedOptions.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
  }

  openFiltersDialog(){  
    const dialogRef = this.dialogService.open(FiltersDialogComponent,{
      minWidth: "500px",
      data: {
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });
    dialogRef.afterClosed().subscribe({
      next: result =>{
        if(result){
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.pageNumber = 1;
          this.getProducts();
        }
      }
    })
  }
}
