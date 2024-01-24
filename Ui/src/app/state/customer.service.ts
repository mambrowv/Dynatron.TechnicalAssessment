import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CustomerState } from "./customer.model";

@Injectable({
    providedIn: 'root'
})
export class CustomerService {
    constructor (private http: HttpClient) {}

    public GetList(): Observable<CustomerState> {
        return this.http.get<CustomerState>('https://localhost:7274/customers');
    }
}