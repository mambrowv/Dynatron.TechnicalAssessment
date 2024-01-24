import { Component } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as CustomerActions from '../../../../state/customer.actions';
import { Customer, CustomerState } from '../../../../state/customer.model';
import { CommonModule } from '@angular/common';
import { getCustomers } from '../../../../state/customer.selectors';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.scss'
})
export class CustomerListComponent {
  customers$: Observable<Customer[]> = this.store.select(getCustomers);

  constructor(private store: Store<CustomerState>) {}

  ngOnInit() {
    this.store.dispatch(CustomerActions.GetList());
  }
}
