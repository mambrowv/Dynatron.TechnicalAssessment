import { createReducer, on } from '@ngrx/store'
import * as CustomerActions from './customer.actions';
import { CustomerState } from './customer.model';

const defaultState: CustomerState = {
    items: [],
    isLoading: false,
    page: 1,
    pageSize: 25,
    totalPages: 0,
    totalRows: 0,
}

export const customerReducer = createReducer(
    defaultState,
    on(CustomerActions.GetList, state => ({ ...state, isLoading: true })),
    on(CustomerActions.GetListSuccess, (state, newState) => {
        console.log(state);
        console.log(newState);
        return { 
            items: newState.customers.items, 
            page: newState.customers.page, 
            pageSize: newState.customers.pageSize,
            totalPages: newState.customers.totalPages,
            totalRows: newState.customers.totalRows,
            selectedCustomer: state.selectedCustomer,
            isLoading: false }
    })
);
