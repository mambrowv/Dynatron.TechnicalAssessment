import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { customerReducer, selectionReducer } from './state/customer.reducer';
import { Store, provideState, provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { CustomerEffects } from './state/customer.effects';

import * as CustomerActions from './state/customer.actions';

export const appConfig: ApplicationConfig = {
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: (store: Store) => {
        return () => {
          store.dispatch(CustomerActions.GetList());
          store.dispatch(CustomerActions.GetSelectedCustomer());
        };
      },
      multi: true,
      deps: [Store]
    },
    provideRouter(routes), 
    provideAnimations(), 
    importProvidersFrom(HttpClientModule),
    provideStore(),
    provideState({ name: 'customers', reducer: customerReducer }),
    provideState({ name: 'customerSelection', reducer: selectionReducer }),
    provideEffects(CustomerEffects)
  ]
};