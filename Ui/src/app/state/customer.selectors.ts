import { createFeatureSelector, createSelector } from "@ngrx/store";
import { Customer, CustomerState } from "./customer.model";

export const customersFeatureSelector = createFeatureSelector<CustomerState>('customers')

export const selectCustomer = (state: CustomerState) => state.selectedCustomer;
export const selectAllCustomers = (state: CustomerState) => state.items;

export const getCustomers = createSelector(customersFeatureSelector, selectAllCustomers)

export const selectCustomers = createSelector(
    selectCustomer,
    selectAllCustomers,
    (selectedCustomer: Customer | undefined, allCustomers: Customer[]) => {
        if (selectedCustomer && allCustomers) 
            return allCustomers.filter((customer: Customer) => customer.customerId === selectedCustomer.customerId);
        
        return allCustomers;
    });