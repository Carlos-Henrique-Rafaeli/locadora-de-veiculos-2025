import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Observer } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  CadastrarGrupoVeiculoModel,
  CadastrarGrupoVeiculoResponseModel,
} from '../grupoVeiculo.models';
import { GrupoVeiculoService } from '../grupoVeiculo.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-cadastrar-grupo-veiculo',
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
  templateUrl: './cadastrar-grupo-veiculo.html',
})
export class CadastrarGrupoVeiculo {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly grupoVeiculoService = inject(GrupoVeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected grupoVeiculoForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  });

  get nome() {
    return this.grupoVeiculoForm.get('nome');
  }

  public cadastrar() {
    if (this.grupoVeiculoForm.invalid) return;

    const grupoVeiculoModel: CadastrarGrupoVeiculoModel = this.grupoVeiculoForm.value;

    const cadastroObserver: Observer<CadastrarGrupoVeiculoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${grupoVeiculoModel.nome}" foi cadastrado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/grupos-veiculos']),
    };

    this.grupoVeiculoService.cadastrar(grupoVeiculoModel).subscribe(cadastroObserver);
  }
}
