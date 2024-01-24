import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { CustomerService } from "./customer.service";
import * as CustomerActions from './customer.actions';
import { EMPTY, catchError, exhaustMap, map, of } from "rxjs";

@Injectable()
export class CustomerEffects {
    constructor(private actions$: Actions, private customerService: CustomerService) { }

    getList$ = createEffect(() => this.actions$.pipe(
        ofType(CustomerActions.GET_LIST),
        exhaustMap(() => this.customerService.GetList()
            .pipe(
                map(data => CustomerActions.GetListSuccess({customers:data})),
                catchError(() => EMPTY))
            )
        )
    );

}