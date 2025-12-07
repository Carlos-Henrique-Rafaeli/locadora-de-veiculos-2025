import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import {
  filter,
  map,
  Observable,
  startWith,
  switchMap,
  tap,
  shareReplay,
  Observer,
  take,
  combineLatest,
} from 'rxjs';
import { ListagemCondutoresModel } from '../../condutor/condutor.models';
import { ListagemGruposVeiculosModel } from '../../grupo-veiculo/grupoVeiculo.models';
import { ListagemPlanosCobrancasModel } from '../../plano-cobranca/planoCobranca.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { ListagemTaxasServicosModel } from '../../taxa-servico/taxaServico.models';
import { ListagemVeiculosModel } from '../../veiculo/veiculo.models';
import {
  DetalhesAluguelModel,
  EditarAluguelModel,
  EditarAluguelResponseModel,
} from '../aluguel.models';
import { AluguelService } from '../aluguel.service';

@Component({
  selector: 'app-editar-aluguel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    RouterLink,
    ReactiveFormsModule,
    AsyncPipe,
  ],
  templateUrl: './editar-aluguel.html',
})
export class EditarAluguel {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly aluguelService = inject(AluguelService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected aluguelForm: FormGroup = this.formBuilder.group({
    condutorId: ['', [Validators.required]],
    grupoVeiculoId: ['', [Validators.required]],
    veiculoId: ['', [Validators.required]],
    dataEntrada: ['', [Validators.required]],
    dataRetorno: ['', [Validators.required]],
    planoCobrancaId: ['', [Validators.required]],
    taxasServicosIds: [[]],
  });

  get condutorId() {
    return this.aluguelForm.get('condutorId');
  }

  get grupoVeiculoId() {
    return this.aluguelForm.get('grupoVeiculoId');
  }

  get veiculoId() {
    return this.aluguelForm.get('veiculoId');
  }

  get dataEntrada() {
    return this.aluguelForm.get('dataEntrada');
  }

  get dataRetorno() {
    return this.aluguelForm.get('dataRetorno');
  }

  get planoCobrancaId() {
    return this.aluguelForm.get('planoCobrancaId');
  }

  get taxasServicosIds() {
    return this.aluguelForm.get('taxasServicosIds');
  }

  protected readonly condutores$ = this.route.data.pipe(
    filter((data) => data['condutores']),
    map((data) => data['condutores'] as ListagemCondutoresModel[]),
  );

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as ListagemGruposVeiculosModel[]),
  );

  protected readonly veiculos$ = this.route.data.pipe(
    filter((data) => data['veiculos']),
    map((data) => data['veiculos'] as ListagemVeiculosModel[]),
  );

  protected readonly planosCobrancas$ = this.route.data.pipe(
    filter((data) => data['planosCobrancas']),
    map((data) => {
      const planos = data['planosCobrancas'] as ListagemPlanosCobrancasModel[];

      return planos.map((p) => ({
        ...p,
        tipoPlano: this.converterTipo(p.tipoPlano),
      }));
    }),
  );

  private converterTipo(plano: string) {
    if (plano === 'PlanoDiario') return 'Plano Diário';
    else if (plano === 'PlanoControlado') return 'Plano Controlado';
    else return 'Plano Livre';
  }

  protected readonly taxasServicos$ = this.route.data.pipe(
    filter((data) => data['taxasServicos']),
    map((data) => data['taxasServicos'] as ListagemTaxasServicosModel[]),
  );

  protected readonly aluguel$ = this.route.data.pipe(
    filter((data) => data['aluguel']),
    map((data) => data['aluguel'] as DetalhesAluguelModel),
    tap((aluguel) =>
      this.aluguelForm.patchValue({
        condutorId: aluguel.condutor.id,
        grupoVeiculoId: aluguel.grupoVeiculo.id,
        veiculoId: aluguel.veiculo.id,
        dataEntrada: aluguel.dataInicio,
        dataRetorno: aluguel.dataFim,
        planoCobrancaId: aluguel.planoCobranca.id,
        taxasServicosIds: aluguel.taxasServicos.map((t) => t.id),
      }),
    ),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly veiculosFiltrados$: Observable<ListagemVeiculosModel[]> = combineLatest([
    this.veiculos$,
    this.aluguelForm.get('grupoVeiculoId')!.valueChanges.pipe(startWith(null)),
    this.aluguel$,
  ]).pipe(
    map(([veiculos, grupoId, aluguel]) => {
      const idParaFiltrar = grupoId ?? aluguel?.grupoVeiculo.id;
      return idParaFiltrar ? veiculos.filter((v) => v.grupoVeiculo.id === idParaFiltrar) : veiculos;
    }),
    tap((filtrados) => {
      if (this.veiculoId?.value && !filtrados.some((v) => v.id === this.veiculoId?.value)) {
        this.veiculoId?.setValue(null);
      }
    }),
  );

  protected readonly planosFiltrados$: Observable<ListagemPlanosCobrancasModel[]> = combineLatest([
    this.planosCobrancas$,
    this.aluguelForm.get('grupoVeiculoId')!.valueChanges.pipe(startWith(null)),
    this.aluguel$,
  ]).pipe(
    map(([planos, grupoId, aluguel]) => {
      const idParaFiltrar = grupoId ?? aluguel?.grupoVeiculo.id;
      return idParaFiltrar ? planos.filter((p) => p.grupoVeiculo.id === idParaFiltrar) : planos;
    }),
    tap((filtrados) => {
      if (
        this.planoCobrancaId?.value &&
        !filtrados.some((v) => v.id === this.planoCobrancaId?.value)
      ) {
        this.planoCobrancaId?.setValue(null);
      }
    }),
  );

  public editar() {
    if (this.aluguelForm.invalid) return;

    const editarAluguelModel: EditarAluguelModel = this.aluguelForm.value;

    const edicaoObserver: Observer<EditarAluguelResponseModel> = {
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
        switchMap((aluguel) => this.aluguelService.editar(aluguel.id, editarAluguelModel)),
      )
      .subscribe(edicaoObserver);
  }
}
