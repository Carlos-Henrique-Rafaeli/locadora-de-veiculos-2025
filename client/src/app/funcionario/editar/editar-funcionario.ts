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
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { NgxMaskDirective } from 'ngx-mask';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { FuncionarioService } from '../funcionario.service';
import { filter, map, tap, shareReplay, Observer, take, switchMap } from 'rxjs';
import {
  DetalhesFuncionarioModel,
  EditarFuncionarioModel,
  EditarFuncionarioResponseModel,
} from '../funcionario.models';

@Component({
  selector: 'app-editar-funcionario',
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
    AsyncPipe,
    NgxMaskDirective,
  ],
  templateUrl: './editar-funcionario.html',
})
export class EditarFuncionario {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly funcionarioService = inject(FuncionarioService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected funcionarioForm: FormGroup = this.formBuilder.group({
    nomeCompleto: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    cpf: ['', [Validators.required, Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
    salario: ['', [Validators.required, Validators.max(999999999.99), Validators.min(0.01)]],
    admissaoEmUtc: ['', [Validators.required]],
  });

  get nomeCompleto() {
    return this.funcionarioForm.get('nomeCompleto');
  }

  get cpf() {
    return this.funcionarioForm.get('cpf');
  }

  get salario() {
    return this.funcionarioForm.get('salario');
  }

  get admissaoEmUtc() {
    return this.funcionarioForm.get('admissaoEmUtc');
  }

  protected readonly funcionario$ = this.route.data.pipe(
    filter((data) => data['funcionario']),
    map((data) => data['funcionario'] as DetalhesFuncionarioModel),
    tap((funcionario) => this.funcionarioForm.patchValue(funcionario)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.funcionarioForm.invalid) return;

    const editarFuncionarioModel: EditarFuncionarioModel = this.funcionarioForm.value;

    const edicaoObserver: Observer<EditarFuncionarioResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarFuncionarioModel.nomeCompleto}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/funcionarios']),
    };

    this.funcionario$
      .pipe(
        take(1),
        switchMap((funcionario) =>
          this.funcionarioService.editar(funcionario.id, editarFuncionarioModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
