import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { NgxMaskDirective } from 'ngx-mask';
import { Observer } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  CadastrarFuncionarioModel,
  CadastrarFuncionarioResponseModel,
} from '../funcionario.models';
import { FuncionarioService } from '../funcionario.service';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-cadastrar-funcionario',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    RouterLink,
    ReactiveFormsModule,
    NgxMaskDirective,
  ],
  templateUrl: './cadastrar-funcionario.html',
})
export class CadastrarFuncionario {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly funcionarioService = inject(FuncionarioService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected funcionarioForm: FormGroup = this.formBuilder.group({
    nomeCompleto: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    cpf: ['', [Validators.required, Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
    confirmarSenha: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
    salario: ['', [Validators.required]],
    admissaoEmUtc: ['', [Validators.required]],
  });

  get nomeCompleto() {
    return this.funcionarioForm.get('nomeCompleto');
  }

  get cpf() {
    return this.funcionarioForm.get('cpf');
  }

  get email() {
    return this.funcionarioForm.get('email');
  }

  get senha() {
    return this.funcionarioForm.get('senha');
  }

  get confirmarSenha() {
    return this.funcionarioForm.get('confirmarSenha');
  }

  get salario() {
    return this.funcionarioForm.get('salario');
  }

  get admissaoEmUtc() {
    return this.funcionarioForm.get('admissaoEmUtc');
  }

  public cadastrar() {
    if (this.funcionarioForm.invalid) return;

    const funcionarioModel: CadastrarFuncionarioModel = this.funcionarioForm.value;

    const cadastroObserver: Observer<CadastrarFuncionarioResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${funcionarioModel.nomeCompleto}" foi cadastrado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/funcionarios']),
    };

    this.funcionarioService.cadastrar(funcionarioModel).subscribe(cadastroObserver);
  }
}
