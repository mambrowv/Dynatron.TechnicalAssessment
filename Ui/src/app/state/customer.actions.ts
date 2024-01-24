import { createAction, props } from "@ngrx/store";
import { Customer, CustomerState } from "./customer.model";


export const GET_LIST = '[Customer] Get List';
export const GetList = createAction(GET_LIST);

export const GET_LIST_SUCCESS = '[Customer] Get List Success';
export const GetListSuccess = createAction(GET_LIST_SUCCESS, props<{customers: CustomerState}>());

export const GET_LIST_FAIL = '[Customer] Get List Fail';
export const GetListFail = createAction(GET_LIST_FAIL, props<{err: any}>());

export const UPDATE = '[Customer] Update';
export const Update = createAction(UPDATE, props<{customer: Customer}>());

export const UPDATE_SUCCESS = '[Customer] Update Success';
export const UpdateSuccess = createAction(UPDATE_SUCCESS, props<{customer: Customer}>());

export const UPDATE_FAIL = '[Customer] Update Fail';
export const UpdateFail = createAction(UPDATE_FAIL, props<{err: any}>());

export const CREATE = '[Customer] Create';
export const Create = createAction(CREATE, props<{customer: Customer}>());

export const CREATE_SUCCESS = '[Customer] Create Success';
export const CreateSuccess = createAction(CREATE_SUCCESS, props<{customer: Customer}>());

export const CREATE_FAIL = '[Customer] Create Fail';
export const CreateFail = createAction(CREATE_FAIL, props<{err: any}>());

export const SELECT = '[Customer] Select';
export const Select = createAction(SELECT, props<{customerId: number}>());