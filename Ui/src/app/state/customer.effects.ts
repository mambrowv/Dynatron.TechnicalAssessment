import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CustomerService } from "./customer.service";
import { EMPTY, catchError, exhaustMap, filter, map, of, switchMap, tap, withLatestFrom } from "rxjs";
import { Store } from "@ngrx/store";
import { CustomerPagination } from "./customer.model";

import * as CustomerActions from './customer.actions';
import * as CustomerSelectors from './customer.selectors';

@Injectable()
export class CustomerEffects {
    constructor(private actions$: Actions, private customerService: CustomerService, private store: Store) { }

    getList$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.GetNextPage),
        withLatestFrom(this.store.select(CustomerSelectors.GetCurrentPage)),
        filter(([_, pagination]: [any, CustomerPagination]) => !pagination.isEndOfPage),
        switchMap(([_, pagination]: [any, CustomerPagination]) => 
            this.customerService.GetList(pagination.page + 1, pagination.pageSize).pipe(
                map(data => CustomerActions.GetNextPageSuccess({ customerState: data })),
                catchError(() => EMPTY)
            ))
    ));

    updateCustomer$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.Update),
        exhaustMap((action) => 
            this.customerService.Update(action.customer).pipe(
                map(data => CustomerActions.UpdateSuccess({ customer: data })),
                catchError(() => EMPTY)
        ))
    ));

    createCustomer$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.Create),
        exhaustMap((action) => 
            this.customerService.Create(action.customer).pipe(
                map(data => CustomerActions.CreateSuccess({ customer: data })),
                catchError(() => EMPTY)
        ))
    ));

    createSuccessCustomer$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.CreateSuccess),
        map((action) =>  CustomerActions.SelectCustomer({ customerId: action.customer.customerId }))
        )
    );

    getSelectedCustomer$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.GetSelectedCustomer),
        map(() => {
            const savedCustomerId = Number(localStorage.getItem(CustomerSelectors.SelectedCustomerIdStorageKey));
            if(isNaN(savedCustomerId) || savedCustomerId <= 0) {
                return CustomerActions.CustomerDeselected();
            }

            return CustomerActions.CustomerSelected({customerId: savedCustomerId});
        })
    ));

    selectCustomer$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.SelectCustomer),
        map((action) => {
            const prevCustomerId = Number(localStorage.getItem(CustomerSelectors.SelectedCustomerIdStorageKey));
            if(!isNaN(prevCustomerId) && prevCustomerId > 0) {
                CustomerActions.CustomerDeselected()
            }

            localStorage.setItem(CustomerSelectors.SelectedCustomerIdStorageKey, action.customerId.toString());
            return CustomerActions.CustomerSelected({customerId: action.customerId});
        })
    ));

    deselectCustomer$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.DeselectCustomer),
        map(() => {
            localStorage.removeItem(CustomerSelectors.SelectedCustomerIdStorageKey);
            return CustomerActions.CustomerDeselected();
        })
    ))
}