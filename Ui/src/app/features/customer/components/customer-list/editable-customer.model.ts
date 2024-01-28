import { Customer } from "../../../../state/customer.model";

export interface EditableCustomer extends Customer {
    isEdit: boolean;
    isSelected: boolean;
}