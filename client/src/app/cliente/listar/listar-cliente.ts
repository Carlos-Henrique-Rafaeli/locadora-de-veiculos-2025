import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { filter, map } from 'rxjs';
import { ClienteService } from '../cliente.service';
import { ListagemClientesModel } from '../cliente.models';

@Component({
  selector: 'app-listar-cliente',
  imports: [MatButtonModule, MatIconModule, MatCardModule, RouterLink, AsyncPipe],
  templateUrl: './listar-cliente.html',
})
export class ListarClientes {
  protected readonly route = inject(ActivatedRoute);
  protected readonly clienteService = inject(ClienteService);

  protected readonly clientes$ = this.route.data.pipe(
    filter((data) => data['clientes']),
    map((data) => data['clientes'] as ListagemClientesModel[]),
  );
}
