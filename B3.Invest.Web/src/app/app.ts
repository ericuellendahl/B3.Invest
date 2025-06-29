import { Component } from '@angular/core';
import { InvestmentForm } from './investment-form/investment-form';
import { InvestmentService } from './investment.service';

@Component({
  selector: 'app-root',
  imports: [InvestmentForm],
  providers: [InvestmentService],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected title = 'b3investmentweb';
}
