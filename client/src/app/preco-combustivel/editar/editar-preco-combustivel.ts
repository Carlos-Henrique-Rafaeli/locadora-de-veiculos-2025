import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  EditarPrecoCombustivelModel,
  ListagemPrecoCombustivelModel,
} from '../precoCombustivel.model';
import { filter, map, tap, shareReplay, Observer, take, switchMap } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { PrecoCombustivelService } from '../precoCombustivel.service';

@Component({
  selector: 'app-editar-preco-combustivel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
    AsyncPipe,
  ],
  templateUrl: './editar-preco-combustivel.html',
})
export class EditarPrecoCombustivel {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly precoCombustivelService = inject(PrecoCombustivelService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected precoCombustivelForm: FormGroup = this.formBuilder.group({
    gasolina: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
    diesel: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
    etanol: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
  });

  get gasolina() {
    return this.precoCombustivelForm.get('gasolina');
  }

  get diesel() {
    return this.precoCombustivelForm.get('diesel');
  }

  get etanol() {
    return this.precoCombustivelForm.get('etanol');
  }

  protected readonly precoCombustivel$ = this.route.data.pipe(
    filter((data) => data['precoCombustivel']),
    map((data) => data['precoCombustivel'] as ListagemPrecoCombustivelModel),
    tap((precoCombustivel) => this.precoCombustivelForm.patchValue(precoCombustivel)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.precoCombustivelForm.invalid) return;

    const editarPrecoCombustivelModel: EditarPrecoCombustivelModel =
      this.precoCombustivelForm.value;

    const edicaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi editado com sucesso!`),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/preco-combustivel']),
    };

    this.precoCombustivel$
      .pipe(
        take(1),
        switchMap(() => this.precoCombustivelService.editar(editarPrecoCombustivelModel)),
      )
      .subscribe(edicaoObserver);
  }
}
