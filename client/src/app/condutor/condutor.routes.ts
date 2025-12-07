import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Routes } from '@angular/router';
import { ListagemCondutoresModel } from './condutor.models';
import { CondutorService } from './condutor.service';
import { ListarCondutores } from './listar/listar-condutores';
import { CadastrarCondutor } from './cadastrar/cadastrar-condutor';
import { ListagemClientesModel } from '../cliente/cliente.models';
import { ClienteService } from '../cliente/cliente.service';
import { EditarCondutor } from './editar/editar-condutor';
import { ExcluirCondutor } from './excluir/excluir-condutor';
import { provideNgxMask } from 'ngx-mask';

const listagemCondutoresResolver: ResolveFn<ListagemCondutoresModel[]> = () => {
  const condutorService = inject(CondutorService);

  return condutorService.selecionarTodas();
};

const detalhesCondutorResolver = (route: ActivatedRouteSnapshot) => {
  const condutorService = inject(CondutorService);

  if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

  const condutorId = route.paramMap.get('id')!;

  return condutorService.selecionarPorId(condutorId);
};

const listagemClientesResolver: ResolveFn<ListagemClientesModel[]> = () => {
  const clienteService = inject(ClienteService);

  return clienteService.selecionarTodas();
};

export const condutorRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarCondutores,
        resolve: { condutores: listagemCondutoresResolver },
      },
      {
        path: 'cadastrar',
        component: CadastrarCondutor,
        resolve: { clientes: listagemClientesResolver },
      },
      {
        path: 'editar/:id',
        component: EditarCondutor,
        resolve: { condutor: detalhesCondutorResolver, clientes: listagemClientesResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirCondutor,
        resolve: { condutor: detalhesCondutorResolver },
      },
    ],
    providers: [CondutorService, ClienteService, provideNgxMask()],
  },
];
