import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { TaxaServicoService } from '../taxaServico.service';
import { filter, map, tap, shareReplay, Observer, take, switchMap } from 'rxjs';
import {
  DetalhesTaxaServicoModel,
  EditarTaxaServicoModel,
  EditarTaxaServicoResponseModel,
} from '../taxaServico.models';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-editar-taxa-servico',
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
  templateUrl: './editar-taxa-servico.html',
})
export class EditarTaxaServico {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly taxaServicoService = inject(TaxaServicoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected taxaServicoForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    valor: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
    tipoCobranca: ['', [Validators.required]],
  });

  get nome() {
    return this.taxaServicoForm.get('nome');
  }

  get valor() {
    return this.taxaServicoForm.get('valor');
  }

  get tipoCobranca() {
    return this.taxaServicoForm.get('tipoCobranca');
  }

  protected readonly tiposCobrancas = [
    { nome: 'Preco Fixo', valor: 'PrecoFixo' },
    { nome: 'Cobrança Diária', valor: 'CobrancaDiaria' },
  ];

  protected readonly taxaServico$ = this.route.data.pipe(
    filter((data) => data['taxaServico']),
    map((data) => data['taxaServico'] as DetalhesTaxaServicoModel),
    tap((taxaServico) => this.taxaServicoForm.patchValue(taxaServico)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.taxaServicoForm.invalid) return;

    const editarTaxaServicoModel: EditarTaxaServicoModel = this.taxaServicoForm.value;

    const edicaoObserver: Observer<EditarTaxaServicoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarTaxaServicoModel.nome}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/taxas-servicos']),
    };

    this.taxaServico$
      .pipe(
        take(1),
        switchMap((taxaServico) =>
          this.taxaServicoService.editar(taxaServico.id, editarTaxaServicoModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
