import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';
import { ListagemAlugueisModel } from './aluguel.models';
import { AluguelService } from './aluguel.service';
import { ListarAlugueis } from './listar/listar-alugueis';

const listagemAlugueisResolver: ResolveFn<ListagemAlugueisModel[]> = () => {
  const aluguelService = inject(AluguelService);

  return aluguelService.selecionarTodas();
};

// const detalhesAluguelResolver = (route: ActivatedRouteSnapshot) => {
//   const aluguelService = inject(AluguelService);

//   if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

//   const aluguelId = route.paramMap.get('id')!;

//   return aluguelService.selecionarPorId(aluguelId);
// };

// const listagemGruposVeiculosResolver: ResolveFn<ListagemGruposVeiculosModel[]> = () => {
//   const grupoVeiculoService = inject(GrupoVeiculoService);

//   return grupoVeiculoService.selecionarTodas();
// };

export const aluguelRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarAlugueis,
        resolve: { alugueis: listagemAlugueisResolver },
      },
      // {
      //   path: 'cadastrar',
      //   component: CadastrarAluguel,
      //   resolve: { gruposVeiculos: listagemGruposVeiculosResolver },
      // },
      // {
      //   path: 'editar/:id',
      //   component: EditarAluguel,
      //   resolve: {
      //     aluguel: detalhesAluguelResolver,
      //     gruposVeiculos: listagemGruposVeiculosResolver,
      //   },
      // },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirAluguel,
      //   resolve: { aluguel: detalhesAluguelResolver },
      // },
    ],
    providers: [AluguelService],
  },
];
