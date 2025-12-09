import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observer, take, switchMap, filter, map, shareReplay, tap } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  DetalhesAluguelModel,
  FinalizarAluguelModel,
  FinalizarAluguelResponseModel,
} from '../aluguel.models';
import { AluguelService } from '../aluguel.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-finalizar-aluguel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './finalizar-aluguel.html',
})
export class FinalizarAluguel {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly aluguelService = inject(AluguelService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected aluguelForm: FormGroup = this.formBuilder.group({
    dataRetorno: ['', [Validators.required]],
    kmInicial: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
    kmAtual: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
    tanqueCheio: [true],
    porcentagemTanque: [],
  });

  get dataRetorno() {
    return this.aluguelForm.get('dataRetorno');
  }

  get kmInicial() {
    return this.aluguelForm.get('kmInicial');
  }

  get kmAtual() {
    return this.aluguelForm.get('kmAtual');
  }

  get tanqueCheio() {
    return this.aluguelForm.get('tanqueCheio');
  }

  get porcentagemTanque() {
    return this.aluguelForm.get('porcentagemTanque');
  }

  protected readonly aluguel$ = this.route.data.pipe(
    filter((data) => data['aluguel']),
    map((data) => data['aluguel'] as DetalhesAluguelModel),
    tap((aluguel) => this.aluguelForm.patchValue(aluguel)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public finalizar() {
    if (this.aluguelForm.invalid) return;

    const finalizarAluguelModel: FinalizarAluguelModel = this.aluguelForm.value;

    if (finalizarAluguelModel.tanqueCheio) finalizarAluguelModel.porcentagemTanque = undefined;

    const edicaoObserver: Observer<FinalizarAluguelResponseModel> = {
      next: () => this.notificacaoService.sucesso(`O registro foi editado com sucesso!`),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/alugueis']),
    };

    this.aluguel$
      .pipe(
        take(1),
        switchMap((aluguel) => this.aluguelService.finalizar(aluguel.id, finalizarAluguelModel)),
      )
      .subscribe(edicaoObserver);
  }
}
