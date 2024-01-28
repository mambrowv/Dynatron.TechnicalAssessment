import { Component, Inject } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { CommonModule } from '@angular/common';
import { Customer } from '../../../../../state/customer.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-customer',
  standalone: true,
  imports: [CommonModule,
    MatFormFieldModule, 
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatDividerModule,
    FormsModule],
  templateUrl: './create-customer.component.html',
  styleUrl: './create-customer.component.scss'
})
export class CreateCustomerComponent {

  constructor(public dialogRef: MatDialogRef<CreateCustomerComponent, Customer>, 
    @Inject(MAT_DIALOG_DATA) public customer: Customer) {}

  save() {
    this.dialogRef.close(this.customer);
  }
}
