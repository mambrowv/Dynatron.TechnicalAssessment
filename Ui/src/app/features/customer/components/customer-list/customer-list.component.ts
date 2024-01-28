import { Component, HostListener } from '@angular/core';
import { Observable, delay, firstValueFrom, forkJoin, iif, map, mergeMap, of, tap } from 'rxjs';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { EditableCustomer } from './editable-customer.model';

import * as CustomerSelectors from '../../../../state/customer.selectors';
import * as CustomerActions from '../../../../state/customer.actions';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [MatTableModule, 
    CommonModule, 
    FormsModule, 
    MatFormFieldModule,
    MatInputModule, 
    MatDatepickerModule, 
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.scss'
})
export class CustomerListComponent {
  
  private initializedDataSource = new MatTableDataSource<EditableCustomer>();
  
  customerTableScheme = [ 'firstName', 'lastName', 'emailAddress', 'createdDateTime', 'updateDateTime', 'isEdit' ];
  customers$: Observable<EditableCustomer[]> = of();
  customerTableDataSource$: Observable<MatTableDataSource<any>> = of();
  isLoading$: Observable<boolean> = of();
  isEndOfPage$: Observable<boolean> = of();

  constructor(private store: Store) {}

  ngOnInit() {
    this.isLoading$ = this.store.select(CustomerSelectors.GetLoadingCustomers)
    this.isEndOfPage$ = this.store.select(CustomerSelectors.GetCustomersEndOfPage)

    this.customers$ = this.store.select(CustomerSelectors.GetCustomers).pipe(
      mergeMap((customers) => 
        this.store.select(CustomerSelectors.GetSelectedCustomerId).pipe(
            map((customerId) => customers.map(c => Object.assign({} as EditableCustomer, c, { isEdit: false, isSelected: customerId == c.customerId }))
          )
        )
      )
    );

    this.customerTableDataSource$ = this.customers$.pipe(
      map(customers => {
        const dataSource = this.initializedDataSource;
        dataSource.data = customers;
        return dataSource;
      })
    );
  }

  selectCustomer(customerId: number) {
    this.store.dispatch(CustomerActions.SelectCustomer({ customerId: customerId }));
  }

  deselectCustomer() {
    this.store.dispatch(CustomerActions.DeselectCustomer());
  }

  updateCustomer(customer: EditableCustomer) {
    customer.isEdit = false;
    this.store.dispatch(CustomerActions.Update({ customer: customer }));
  }

  @HostListener("window:scroll", ["$event"])
  async onWindowScroll() {
    const viewPosition = (document.body.scrollTop || document.documentElement.scrollTop) + document.documentElement.offsetHeight;
    const bottomOfWindow = document.documentElement.scrollHeight - 25;

    const isLoading = await firstValueFrom(this.isLoading$);
    const isEndOfPage = await firstValueFrom(this.isEndOfPage$);

    if(!isLoading && !isEndOfPage && (viewPosition / bottomOfWindow >= .95)) {
      this.store.dispatch(CustomerActions.GetList());
    }
  }
}