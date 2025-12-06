import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';
import { ListagemClientesModel } from '../../cliente/cliente.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  DetalhesCondutorModel,
  EditarCondutorModel,
  EditarCondutorResponseModel,
} from '../condutor.models';
import { CondutorService } from '../condutor.service';

@Component({
  selector: 'app-editar-condutor',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    RouterLink,
    ReactiveFormsModule,
    AsyncPipe,
  ],
  templateUrl: './editar-condutor.html',
})
export class EditarCondutor {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly condutorService = inject(CondutorService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected condutorForm: FormGroup = this.formBuilder.group({
    clienteId: ['', [Validators.required]],
    clienteCondutor: [''],
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
    cpf: ['', [Validators.required, Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
    cnh: ['', [Validators.required, Validators.pattern(/^\d{11}$/)]],
    validadeCnh: ['', [Validators.required]],
    telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\) 9\d{4}-\d{4}$/)]],
  });

  get clienteId() {
    return this.condutorForm.get('clienteId');
  }

  get clienteCondutor() {
    return this.condutorForm.get('clienteCondutor');
  }

  get nome() {
    return this.condutorForm.get('nome');
  }

  get email() {
    return this.condutorForm.get('email');
  }

  get cpf() {
    return this.condutorForm.get('cpf');
  }

  get cnh() {
    return this.condutorForm.get('cnh');
  }

  get validadeCnh() {
    return this.condutorForm.get('validadeCnh');
  }

  get telefone() {
    return this.condutorForm.get('telefone');
  }

  protected readonly clientes$ = this.route.data.pipe(
    filter((data) => data['clientes']),
    map((data) => data['clientes'] as ListagemClientesModel[]),
  );

  protected readonly condutor$ = this.route.data.pipe(
    filter((data) => data['condutor']),
    map((data) => data['condutor'] as DetalhesCondutorModel),
    tap((condutor) =>
      this.condutorForm.patchValue({
        clienteId: condutor.cliente.id,
        clienteCondutor: condutor.clienteCondutor,
        nome: condutor.nome,
        email: condutor.email,
        cpf: condutor.cpf,
        cnh: condutor.cnh,
        validadeCnh: condutor.validadeCnh,
        telefone: condutor.telefone,
      }),
    ),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.condutorForm.invalid) return;

    const editarCondutorModel: EditarCondutorModel = this.condutorForm.value;

    if (!editarCondutorModel.clienteCondutor) editarCondutorModel.clienteCondutor = false;

    const edicaoObserver: Observer<EditarCondutorResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarCondutorModel.nome}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/condutores']),
    };

    this.condutor$
      .pipe(
        take(1),
        switchMap((condutor) => this.condutorService.editar(condutor.id, editarCondutorModel)),
      )
      .subscribe(edicaoObserver);
  }
}
