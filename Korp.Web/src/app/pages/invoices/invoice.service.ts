import { Injectable } from "@angular/core";
import { BaseService } from "../../core/base.service";
import { environment } from "../../enviroment";
import { catchError, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Invoice } from "./invoice.model";
import { DefaultListResponse } from "../../core/models/default-list-response.model";

@Injectable({
  providedIn: 'root',
})
export class InvoiceService extends BaseService {
  private url: string = `${environment.invoiceServiceUrl}/invoices`

  constructor(private http: HttpClient) { super() }

    getAll(): Observable<DefaultListResponse<Invoice>> {
        return this.http
            .get<DefaultListResponse<Invoice>>(this.url)
            .pipe(catchError(this.catchProblemDetailsError))
    }
    
    // print(invoice: Invoice): Observable<Pick<Invoice, 'id'>> {
    //     return this.http
    //         .post<Pick<Invoice, 'id'>>(this.url + '/' + invoice.id + '/close', null)
    //         .pipe(catchError(this.catchProblemDetailsError))
    // }
}