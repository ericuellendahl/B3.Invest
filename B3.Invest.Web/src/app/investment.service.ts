import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Investment {
  initialAmount: number;
  months: number;
}

export interface InvestmentResult {
  grossAmount: number;
  netAmount: number;
}

@Injectable({
  providedIn: 'root',
})
export class InvestmentService {
  private readonly apiUrl = 'http://localhost:5212/api/Investment/calculate';

  constructor(private readonly http: HttpClient) {}

  calculateInvestment(investment: Investment): Observable<InvestmentResult> {
    return this.http.post<InvestmentResult>(this.apiUrl, investment);
  }
}
