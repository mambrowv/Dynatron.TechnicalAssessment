export interface Customer {
    customerId: number;
    firstName: string;
    lastName: string;
    emailAddress: string;
    updatedDateTime: Date;
    createdDateTime: Date;
}

export interface CustomerState {
    selectedCustomer?: Customer;
    isLoading: boolean;
    items: Customer[];
    pageSize: number;
    page: number;
    totalRows: number;
    totalPages: number;
}