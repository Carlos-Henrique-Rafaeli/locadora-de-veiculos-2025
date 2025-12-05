import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';
import { ListagemClientesModel } from './cliente.models';
import { ClienteService } from './cliente.service';
import { ListarClientes } from './listar/listar-cliente';

const listagemClientesResolver: ResolveFn<ListagemClientesModel[]> = () => {
  const clienteService = inject(ClienteService);

  return clienteService.selecionarTodas();
};

export const clienteRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarClientes,
        resolve: { clientes: listagemClientesResolver },
      },
    ],
    providers: [ClienteService],
  },
];
