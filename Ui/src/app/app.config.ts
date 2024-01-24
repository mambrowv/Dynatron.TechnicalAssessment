import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { customerReducer } from './state/customer.reducer';
import { provideState, provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { CustomerEffects } from './state/customer.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideAnimations(), 
    importProvidersFrom(HttpClientModule),
    provideStore(),
    provideState({ name: 'customers', reducer: customerReducer }),
    provideEffects(CustomerEffects)
  ]
};