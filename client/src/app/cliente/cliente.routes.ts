import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Routes } from '@angular/router';
import { ListagemClientesModel } from './cliente.models';
import { ClienteService } from './cliente.service';
import { ListarClientes } from './listar/listar-cliente';
import { CadastrarCliente } from './cadastrar/cadastrar-cliente';
import { EditarCliente } from './editar/editar-cliente';

const listagemClientesResolver: ResolveFn<ListagemClientesModel[]> = () => {
  const clienteService = inject(ClienteService);

  return clienteService.selecionarTodas();
};

const detalhesClienteResolver = (route: ActivatedRouteSnapshot) => {
  const clienteService = inject(ClienteService);

  if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

  const clienteId = route.paramMap.get('id')!;

  return clienteService.selecionarPorId(clienteId);
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
      { path: 'cadastrar', component: CadastrarCliente },
      {
        path: 'editar/:id',
        component: EditarCliente,
        resolve: { cliente: detalhesClienteResolver },
      },
    ],
    providers: [ClienteService],
  },
];
