import { Routes } from '@angular/router';
import { CustomerListComponent } from './features/customer/components/customer-list/customer-list.component';

export const routes: Routes = [
    { 
        path: '', 
        component: CustomerListComponent,
    },
];
