import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { InvestmentForm } from './app/investment-form/investment-form';

bootstrapApplication(InvestmentForm, appConfig)
  .catch((err) => console.error(err));
  