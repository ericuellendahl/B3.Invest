import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import { InvestmentService } from '../investment.service';

@Component({
  selector: 'app-investment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './investment-form.html',
  styleUrls: ['./investment-form.css'],
})
export class InvestmentForm {
  form: FormGroup;
  formattedResult: { grossAmount: number; netAmount: number } | null = null;
  validationErrors: { [key: string]: string[] } = {};

  constructor(
    private fb: FormBuilder,
    private cdRef: ChangeDetectorRef,
    private investmentService: InvestmentService
  ) {
    this.form = this.fb.group({
      initialAmount: [null],
      months: [null],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.investmentService.calculateInvestment(this.form.value).subscribe({
        next: (response) => {
          this.formattedResult = response;
          this.validationErrors = {};
          this.cdRef.detectChanges();
          try {
            this.formattedResult = response;
          } catch (err) {
            this.formattedResult = null;
          }
        },
        error: (err) => {
          console.error('Erro ao calcular investimento:', err);
          this.formattedResult = null;
          const errors = err?.error?.errors;
          if (errors && typeof errors === 'object') {
            this.validationErrors = errors;
          } else {
            this.validationErrors = {};
          }

          this.cdRef.detectChanges();
        },
      });
    }
  }
}
