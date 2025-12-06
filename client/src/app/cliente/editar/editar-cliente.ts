import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  DetalhesClienteModel,
  EditarClienteModel,
  EditarClienteResponseModel,
} from '../cliente.models';
import { ClienteService } from '../cliente.service';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-editar-cliente',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    RouterLink,
    AsyncPipe,
    ReactiveFormsModule,
  ],
  templateUrl: './editar-cliente.html',
})
export class EditarCliente {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly clienteService = inject(ClienteService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected clienteForm: FormGroup = this.formBuilder.group({
    tipoCliente: ['', [Validators.required]],
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\) 9\d{4}-\d{4}$/)]],
    cpf: [''],
    cnpj: [''],
    estado: ['', [Validators.required]],
    cidade: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    bairro: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    rua: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    numero: ['', [Validators.required]],
  });

  get tipoCliente() {
    return this.clienteForm.get('tipoCliente');
  }

  get nome() {
    return this.clienteForm.get('nome');
  }

  get telefone() {
    return this.clienteForm.get('telefone');
  }

  get cpf() {
    return this.clienteForm.get('cpf');
  }

  get cnpj() {
    return this.clienteForm.get('cnpj');
  }

  get estado() {
    return this.clienteForm.get('estado');
  }

  get cidade() {
    return this.clienteForm.get('cidade');
  }

  get bairro() {
    return this.clienteForm.get('bairro');
  }

  get rua() {
    return this.clienteForm.get('rua');
  }

  get numero() {
    return this.clienteForm.get('numero');
  }

  protected readonly cliente$ = this.route.data.pipe(
    filter((data) => data['cliente']),
    map((data) => {
      const cliente = data['cliente'] as DetalhesClienteModel;

      if (cliente.tipoCliente === 'PessoaFisica') {
        cliente.tipoCliente = 'Pessoa Física';
        cliente.cnpj = '';
      } else {
        cliente.tipoCliente = 'Pessoa Jurídica';
        cliente.cpf = '';
      }

      return cliente;
    }),
    tap((cliente) => this.clienteForm.patchValue(cliente)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly tiposClientes = ['Pessoa Física', 'Pessoa Jurídica'];

  protected readonly estados = [
    'AC',
    'AL',
    'AP',
    'AM',
    'BA',
    'CE',
    'DF',
    'ES',
    'GO',
    'MA',
    'MT',
    'MS',
    'MG',
    'PA',
    'PB',
    'PR',
    'PE',
    'PI',
    'RJ',
    'RN',
    'RS',
    'RO',
    'RR',
    'SC',
    'SP',
    'SE',
    'TO',
  ];

  public editar() {
    if (this.clienteForm.invalid) return;

    const editarClienteModel: EditarClienteModel = this.clienteForm.value;

    if (editarClienteModel.tipoCliente === 'Pessoa Jurídica') {
      editarClienteModel.tipoCliente = 'PessoaJuridica';
      editarClienteModel.cpf = undefined;
    } else {
      editarClienteModel.tipoCliente = 'PessoaFisica';
      editarClienteModel.cnpj = undefined;
    }

    const edicaoObserver: Observer<EditarClienteResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarClienteModel.nome}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/clientes']),
    };

    this.cliente$
      .pipe(
        take(1),
        switchMap((cliente) => this.clienteService.editar(cliente.id, editarClienteModel)),
      )
      .subscribe(edicaoObserver);
  }
}
