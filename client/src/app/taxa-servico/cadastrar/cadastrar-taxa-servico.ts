import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterLink, Router } from '@angular/router';
import { Observer } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  CadastrarTaxaServicoModel,
  CadastrarTaxaServicoResponseModel,
} from '../taxaServico.models';
import { TaxaServicoService } from '../taxaServico.service';

@Component({
  selector: 'app-cadastrar-taxa-servico',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './cadastrar-taxa-servico.html',
})
export class CadastrarTaxaServico {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly taxaServicoService = inject(TaxaServicoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected taxaServicoForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    valor: ['', [Validators.required]],
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

  public cadastrar() {
    if (this.taxaServicoForm.invalid) return;

    const taxaServicoModel: CadastrarTaxaServicoModel = this.taxaServicoForm.value;

    const cadastroObserver: Observer<CadastrarTaxaServicoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${taxaServicoModel.nome}" foi cadastrado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/taxas-servicos']),
    };

    this.taxaServicoService.cadastrar(taxaServicoModel).subscribe(cadastroObserver);
  }
}
