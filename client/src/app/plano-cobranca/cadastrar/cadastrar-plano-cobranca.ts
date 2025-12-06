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
import { filter, map, Observer } from 'rxjs';
import { ListagemGruposVeiculosModel } from '../../grupo-veiculo/grupoVeiculo.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  CadastrarPlanoCobrancaModel,
  CadastrarPlanoCobrancaResponseModel,
} from '../planoCobranca.models';
import { PlanoCobrancaService } from '../planoCobranca.service';

@Component({
  selector: 'app-cadastrar-plano-cobranca',
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
  templateUrl: './cadastrar-plano-cobranca.html',
})
export class CadastrarPlanoCobranca {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected planoCobrancaForm: FormGroup = this.formBuilder.group({
    tipoPlano: ['', [Validators.required]],
    grupoVeiculoId: ['', [Validators.required]],
    valorDiario: [null],
    valorKm: [null],
    kmIncluso: [null, Validators.pattern(/^\d+$/)],
    valorKmExcedente: [null],
    valorFixo: [null],
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

  protected readonly tiposPlanos = [
    { nome: 'Plano Diário', valor: 'PlanoDiario' },
    { nome: 'Plano Controlado', valor: 'PlanoControlado' },
    { nome: 'Plano Livre', valor: 'PlanoLivre' },
  ];

  public cadastrar() {
    if (this.planoCobrancaForm.invalid) return;

    const planoCobrancaModel: CadastrarPlanoCobrancaModel = this.planoCobrancaForm.value;

    const cadastroObserver: Observer<CadastrarPlanoCobrancaResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${planoCobrancaModel.tipoPlano}" foi cadastrado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/planos-cobrancas']),
    };

    this.planoCobrancaService.cadastrar(planoCobrancaModel).subscribe(cadastroObserver);
  }
}
