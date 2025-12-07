import { AsyncPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FuncionarioService } from '../funcionario.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs';
import { ListagemFuncionariosModel } from '../funcionario.models';

@Component({
  selector: 'app-listar-funcionarios',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    RouterLink,
    AsyncPipe,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './listar-funcionarios.html',
})
export class ListarFuncionarios {
  protected readonly route = inject(ActivatedRoute);
  protected readonly funcionarioService = inject(FuncionarioService);

  protected readonly funcionarios$ = this.route.data.pipe(
    filter((data) => data['funcionarios']),
    map((data) => data['funcionarios'] as ListagemFuncionariosModel[]),
  );
}
