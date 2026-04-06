import { Injectable } from "@angular/core";
import { BaseService } from "../../core/base.service";
import { environment } from "../../enviroment";
import { catchError, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Product } from "./product.model";
import { DefaultListResponse } from "../../core/models/default-list-response.model";

@Injectable({
  providedIn: 'root',
})
export class ProductService extends BaseService {
    private url: string = `${environment.inventoryServiceApiUrl}/product`

    constructor(private http : HttpClient) { super() }

    getAll(): Observable<DefaultListResponse<Product>> {
        return this.http
            .get<DefaultListResponse<Product>>(this.url)
            .pipe(catchError(this.catchProblemDetailsError))
    }
}