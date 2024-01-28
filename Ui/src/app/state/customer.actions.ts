import { createAction, props } from "@ngrx/store";
import { Customer, CustomerState } from "./customer.model";

export const GetNextPage = createAction('[Customer] Get Next Page');
export const GetNextPageSuccess = createAction('[Customer] Get Next Page Success', props<{customerState: CustomerState}>());

export const Update = createAction('[Customer] Update', props<{customer: Customer}>());
export const UpdateSuccess = createAction('[Customer] Update Success', props<{customer: Customer}>());

export const Create = createAction('[Customer] Create', props<{customer: Customer}>());
export const CreateSuccess = createAction('[Customer] Create Success', props<{customer: Customer}>());

export const GetSelectedCustomer = createAction('[Customer Selection] Get Selected Customer');
export const GetSelectedCustomerSuccess = createAction('[Customer Selection] Get Selected Customer Success', props<{customerId: number}>());
export const SelectCustomer = createAction('[Customer Selection] Select', props<{customerId: number}>());
export const CustomerSelected = createAction('[Customer Selection] Selected', props<{customerId: number}>());
export const DeselectCustomer = createAction('[Customer Selection] Deselect');
export const CustomerDeselected = createAction('[Customer Selection] Deselected');