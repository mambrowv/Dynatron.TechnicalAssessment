import { createFeatureSelector, createSelector } from "@ngrx/store";
import { CustomerPagination, CustomerSelectionState, CustomerState } from "./customer.model";

export const SelectedCustomerIdStorageKey = 'dynatron.customers.selectedCustomerId';

export const customersFeatureSelector = createFeatureSelector<CustomerState>('customers')
export const customerSelectionFeatureSelector = createFeatureSelector<CustomerSelectionState>('customerSelection')

export const selectCurrentPage = (state: CustomerState) => ({ 
    page: state.page, 
    pageSize: state.pageSize, 
    isLoading: state.isLoading, 
    isEndOfPage: state.isEndOfPage 
} as CustomerPagination);

export const GetCurrentPage = createSelector(customersFeatureSelector, selectCurrentPage);
export const GetCustomers = createSelector(customersFeatureSelector, (state: CustomerState) => state.items);
export const GetLoadingCustomers = createSelector(customersFeatureSelector, (state: CustomerState) => state.isLoading);
export const GetCustomersEndOfPage = createSelector(customersFeatureSelector, (state: CustomerState) => state.isEndOfPage);
export const GetSelectedCustomerId = createSelector(customerSelectionFeatureSelector, (state: CustomerSelectionState) => state.customerId);