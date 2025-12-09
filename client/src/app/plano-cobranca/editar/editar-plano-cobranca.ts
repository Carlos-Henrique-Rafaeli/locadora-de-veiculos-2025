import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';
import { ListagemGruposVeiculosModel } from '../../grupo-veiculo/grupoVeiculo.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { PlanoCobrancaService } from '../planoCobranca.service';
import {
  DetalhesPlanoCobrancaModel,
  EditarPlanoCobrancaModel,
  EditarPlanoCobrancaResponseModel,
} from '../planoCobranca.models';

@Component({
  selector: 'app-editar-plano-cobranca',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    RouterLink,
    ReactiveFormsModule,
    AsyncPipe,
  ],
  templateUrl: './editar-plano-cobranca.html',
})
export class EditarPlanoCobranca {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected planoCobrancaForm: FormGroup = this.formBuilder.group({
    tipoPlano: ['', [Validators.required]],
    grupoVeiculoId: ['', [Validators.required]],
    valorDiario: [null, Validators.max(999999999.99), Validators.min(0.01)],
    valorKm: [null, Validators.max(999999999)],
    kmIncluso: [null, Validators.pattern(/^\d+$/), Validators.max(999999999), Validators.min(1)],
    valorKmExcedente: [null, Validators.max(999999999.99), Validators.min(0.01)],
    valorFixo: [null, Validators.max(999999999.99), Validators.min(0.01)],
  });

  get tipoPlano() {
    return this.planoCobrancaForm.get('tipoPlano');
  }

  get grupoVeiculoId() {
    return this.planoCobrancaForm.get('grupoVeiculoId');
  }

  get valorDiario() {
    return this.planoCobrancaForm.get('valorDiario');
  }

  get valorKm() {
    return this.planoCobrancaForm.get('valorKm');
  }

  get kmIncluso() {
    return this.planoCobrancaForm.get('kmIncluso');
  }

  get valorKmExcedente() {
    return this.planoCobrancaForm.get('valorKmExcedente');
  }

  get valorFixo() {
    return this.planoCobrancaForm.get('valorFixo');
  }

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as ListagemGruposVeiculosModel[]),
  );

  protected readonly planoCobranca$ = this.route.data.pipe(
    filter((data) => data['planoCobranca']),
    map((data) => data['planoCobranca'] as DetalhesPlanoCobrancaModel),
    tap((planoCobranca) =>
      this.planoCobrancaForm.patchValue({
        ...planoCobranca,
        grupoVeiculoId: planoCobranca.grupoVeiculo.id,
      }),
    ),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly tiposPlanos = [
    { nome: 'Plano Diário', valor: 'PlanoDiario' },
    { nome: 'Plano Controlado', valor: 'PlanoControlado' },
    { nome: 'Plano Livre', valor: 'PlanoLivre' },
  ];

  public editar() {
    if (this.planoCobrancaForm.invalid) return;

    const editarPlanoCobrancaModel: EditarPlanoCobrancaModel = this.planoCobrancaForm.value;

    const edicaoObserver: Observer<EditarPlanoCobrancaResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarPlanoCobrancaModel.tipoPlano}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/planos-cobrancas']),
    };

    this.planoCobranca$
      .pipe(
        take(1),
        switchMap((planoCobranca) =>
          this.planoCobrancaService.editar(planoCobranca.id, editarPlanoCobrancaModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
