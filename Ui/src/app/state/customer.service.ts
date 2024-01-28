import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Customer, CustomerState } from "./customer.model";

@Injectable({
    providedIn: 'root'
})
export class CustomerService {
    constructor (private http: HttpClient) {}

    public GetList(page: number, pageSize: number): Observable<CustomerState> {
        return this.http.get<CustomerState>(`https://localhost:7274/customers?page=${page}&pageSize=${pageSize}`);
    }

    public Update(customer: Customer): Observable<Customer> {
        return this.http.put<Customer>(`https://localhost:7274/customers/${customer.customerId}`, customer);
    }

    public Create(customer: Customer): Observable<Customer> {
        return this.http.post<Customer>('https://localhost:7274/customers/', customer);
    }
}