export interface Customer {
    customerId: number;
    firstName: string;
    lastName: string;
    emailAddress: string;
    updateDateTime: Date;
    createdDateTime: Date;
}

export interface CustomerState {
    selectedCustomer?: Customer;
    items: Customer[];
    pageSize: number;
    page: number;
    totalRows: number;
    totalPages: number;
    isLoading: boolean;
    isEndOfPage: boolean;
}

export interface CustomerSelectionState {
    customerId: number;
    isSelecting: boolean;
}

export interface CustomerPagination {
    pageSize: number;
    page: number;
    isLoading: boolean;
    isEndOfPage: boolean;
}