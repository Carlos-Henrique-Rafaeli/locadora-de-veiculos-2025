import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';
import { ListarPrecoCombustivel } from './listar/listar-preco-combustivel';
import { ListagemPrecoCombustivelModel } from './precoCombustivel.model';
import { PrecoCombustivelService } from './precoCombustivel.service';
import { EditarPrecoCombustivel } from './editar/editar-preco-combustivel';

const listagemPrecoCombustivelResolver: ResolveFn<ListagemPrecoCombustivelModel> = () => {
  const planosCobrancaService = inject(PrecoCombustivelService);

  return planosCobrancaService.selecionar();
};

export const precoCombustivelRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarPrecoCombustivel,
        resolve: { precoCombustivel: listagemPrecoCombustivelResolver },
      },
      {
        path: 'editar',
        component: EditarPrecoCombustivel,
        resolve: { precoCombustivel: listagemPrecoCombustivelResolver },
      },
    ],
    providers: [PrecoCombustivelService],
  },
];
