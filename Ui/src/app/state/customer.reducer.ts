import { createReducer, on } from '@ngrx/store'
import { CustomerSelectionState, CustomerState } from './customer.model';

import * as CustomerActions from './customer.actions';

const defaultState: CustomerState = {
    items: [],
    page: 0,
    pageSize: 25,
    totalPages: 0,
    totalRows: 0,
    isLoading: false,
    isEndOfPage: false,
}

export const customerReducer = createReducer(
    defaultState,
    on(CustomerActions.GetNextPage, state => ({ ...state, isLoading: true })),
    on(CustomerActions.Update, state => ({ ...state, isLoading: true })),
    on(CustomerActions.Create, state => ({ ...state, isLoading: true })),
    on(CustomerActions.GetNextPageSuccess, (state, newState) => ({
            ...state, 
            ...newState.customerState, 
            items: [...state.items, ...newState.customerState.items], 
            isLoading: false,
            isEndOfPage: newState.customerState.items.length == 0
        })
    ),
    on(CustomerActions.UpdateSuccess, (state, newState) => { 
        let customers = state.items.map(cust => cust.customerId === newState.customer.customerId ? newState.customer : cust);
        return { ...state, items: customers, isLoading: false }
    }),
    on(CustomerActions.CreateSuccess, (state, newState) => { 
        let customers = [...state.items, newState.customer];
        return { ...state, items: customers, isLoading: false }
    }),
);

const defaultSelectionState: CustomerSelectionState = {
    customerId: 0,
    isSelecting: false
}

export const selectionReducer = createReducer(
    defaultSelectionState,
    on(CustomerActions.GetSelectedCustomer, (state, newState) => ({ ...state, ...newState, isSelecting: true })),
    on(CustomerActions.SelectCustomer, state => ({ ...state, isSelecting: true })),
    on(CustomerActions.CustomerSelected, (state, newState) => ({ ...state, ...newState, isSelecting: false })),
    on(CustomerActions.DeselectCustomer, state => ({ ...state, isSelecting: true })),
    on(CustomerActions.CustomerDeselected, state => ({ ...state, customerId: 0, isSelecting: false })),
);