import { Component, OnInit } from "@angular/core";
import { Product } from "./product.model";
import { ProductService } from "./product.service";

@Component({
  selector: 'app-product-page',
//   imports: [CommonModule, MatTableModule, MatIconModule, MatPaginatorModule, MatButtonModule ],
  standalone: true,
  templateUrl: './product.page.html',
  styleUrl: './product.page.css'
})
export class ProductPage implements OnInit {
    products?: Product[]

    constructor(private _productService: ProductService) { }

    ngOnInit(): void {
        
    }

    getProducts(pageIndex: number = 0, pageSize: number = 5): void {
        this._productService
            .getAll()
            .subscribe({
                next: res => this.products = res.data,
                error: () => alert('Erro ao carregar produtos:')
            })
    }
}