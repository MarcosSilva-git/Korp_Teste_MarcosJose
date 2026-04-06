import { Injectable } from "@angular/core";
import { BaseService } from "../../core/base.service";
import { environment } from "../../enviroment";
import { catchError, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { CreateOrUpdateProduct, Product } from "./product.model";
import { DefaultListResponse } from "../../core/models/default-list-response.model";

@Injectable({
  providedIn: 'root',
})
export class ProductService extends BaseService {
    private url: string = `${environment.inventoryServiceApiUrl}/products`

    constructor(private http : HttpClient) { super() }

    getAll(): Observable<DefaultListResponse<Product>> {
        return this.http
            .get<DefaultListResponse<Product>>(this.url)
            .pipe(catchError(this.catchProblemDetailsError))
    }

    create(product: CreateOrUpdateProduct): Observable<Product> {
        return this.http
            .post<Product>(this.url, product)
            .pipe(catchError(this.catchProblemDetailsError))
    }

    update(product: CreateOrUpdateProduct): Observable<Product> {
        return this.http
            .patch<Product>(this.url + '/' + product.id, product)
            .pipe(catchError(this.catchProblemDetailsError))
    }

    delete(id: number): Observable<void> {
        return this.http
            .delete<void>(this.url + '/' + id)
            .pipe(catchError(this.catchProblemDetailsError))
    }
}