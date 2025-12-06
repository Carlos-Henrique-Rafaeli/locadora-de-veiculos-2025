import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';
import { ListagemGruposVeiculosModel } from './grupoVeiculo.models';
import { GrupoVeiculoService } from './grupoVeiculo.service';
import { ListarGruposVeiculos } from './listar/listar-grupos-veiculos';

const listagemGruposVeiculosResolver: ResolveFn<ListagemGruposVeiculosModel[]> = () => {
  const grupoVeiculoService = inject(GrupoVeiculoService);

  return grupoVeiculoService.selecionarTodas();
};

// const detalhesGrupoVeiculoResolver = (route: ActivatedRouteSnapshot) => {
//   const grupoVeiculoService = inject(GrupoVeiculoService);

//   if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

//   const grupoVeiculoId = route.paramMap.get('id')!;

//   return grupoVeiculoService.selecionarPorId(grupoVeiculoId);
// };

export const grupoVeiculoRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarGruposVeiculos,
        resolve: { gruposVeiculos: listagemGruposVeiculosResolver },
      },
      // { path: 'cadastrar', component: CadastrarGrupoVeiculo },
      // {
      //   path: 'editar/:id',
      //   component: EditarGrupoVeiculo,
      //   resolve: { grupoVeiculo: detalhesGrupoVeiculoResolver },
      // },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirGrupoVeiculo,
      //   resolve: { grupoVeiculo: detalhesGrupoVeiculoResolver },
      // },
    ],
    providers: [GrupoVeiculoService],
  },
];
