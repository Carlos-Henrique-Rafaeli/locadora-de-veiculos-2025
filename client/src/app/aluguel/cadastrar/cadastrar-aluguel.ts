import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { filter, map, Observable, Observer, startWith, switchMap, tap } from 'rxjs';
import { ListagemCondutoresModel } from '../../condutor/condutor.models';
import { ListagemGruposVeiculosModel } from '../../grupo-veiculo/grupoVeiculo.models';
import { ListagemPlanosCobrancasModel } from '../../plano-cobranca/planoCobranca.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { ListagemTaxasServicosModel } from '../../taxa-servico/taxaServico.models';
import { ListagemVeiculosModel } from '../../veiculo/veiculo.models';
import { CadastrarAluguelModel, CadastrarAluguelResponseModel } from '../aluguel.models';
import { AluguelService } from '../aluguel.service';

@Component({
  selector: 'app-cadastrar-aluguel',
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
  templateUrl: './cadastrar-aluguel.html',
})
export class CadastrarAluguel {
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

  protected readonly veiculosFiltrados$: Observable<ListagemVeiculosModel[]> = this.aluguelForm
    .get('grupoVeiculoId')!
    .valueChanges.pipe(
      startWith(this.aluguelForm.get('grupoVeiculoId')!.value),
      switchMap((grupoId) =>
        this.veiculos$.pipe(
          map((veiculos) =>
            grupoId ? veiculos.filter((v) => v.grupoVeiculo.id === grupoId) : veiculos,
          ),
          tap(() => this.veiculoId?.setValue(null)),
        ),
      ),
    );

  protected readonly planosFiltrados$: Observable<ListagemPlanosCobrancasModel[]> = this.aluguelForm
    .get('grupoVeiculoId')!
    .valueChanges.pipe(
      startWith(this.aluguelForm.get('grupoVeiculoId')!.value),
      switchMap((grupoId) =>
        this.planosCobrancas$.pipe(
          map((planos) => (grupoId ? planos.filter((p) => p.grupoVeiculo.id === grupoId) : planos)),
          tap(() => this.planoCobrancaId?.setValue(null)),
        ),
      ),
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

  public cadastrar() {
    if (this.aluguelForm.invalid) return;

    const aluguelModel: CadastrarAluguelModel = this.aluguelForm.value;

    const cadastroObserver: Observer<CadastrarAluguelResponseModel> = {
      next: () => this.notificacaoService.sucesso(`O registro foi cadastrado com sucesso!`),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/alugueis']),
    };

    this.aluguelService.cadastrar(aluguelModel).subscribe(cadastroObserver);
  }
}
