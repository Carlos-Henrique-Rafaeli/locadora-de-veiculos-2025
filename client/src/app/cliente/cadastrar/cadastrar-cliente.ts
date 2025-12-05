import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Observer } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { ClienteService } from '../cliente.service';
import { CadastrarClienteModel, CadastrarClienteResponseModel } from '../cliente.models';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-cadastrar-cliente',
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
  templateUrl: './cadastrar-cliente.html',
})
export class CadastrarCliente {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly clienteService = inject(ClienteService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected clienteForm: FormGroup = this.formBuilder.group({
    tipoCliente: ['', [Validators.required]],
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    telefone: ['', [Validators.required, Validators.pattern(/^\(\d{2}\) 9\d{4}-\d{4}$/)]],
    cpf: ['', [Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
    cnpj: ['', [Validators.pattern(/^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$/)]],
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
    return this.clienteForm.get('nome');
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

  public cadastrar() {
    if (this.clienteForm.invalid) return;

    const clienteModel: CadastrarClienteModel = this.clienteForm.value;

    if (clienteModel.tipoCliente === 'Pessoa Jurídica') clienteModel.tipoCliente = 'PessoaJuridica';
    else clienteModel.tipoCliente = 'PessoaFisica';

    const cadastroObserver: Observer<CadastrarClienteResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${clienteModel.nome}" foi cadastrado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/clientes']),
    };

    this.clienteService.cadastrar(clienteModel).subscribe(cadastroObserver);
  }
}
