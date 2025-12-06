import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';
import { ListarVeiculos } from './listar/listar-veiculos';
import { ListagemVeiculosModel } from './veiculo.models';
import { VeiculoService } from './veiculo.service';

const listagemVeiculosResolver: ResolveFn<ListagemVeiculosModel[]> = () => {
  const veiculoService = inject(VeiculoService);

  return veiculoService.selecionarTodas();
};

// const detalhesVeiculoResolver = (route: ActivatedRouteSnapshot) => {
//   const veiculoService = inject(VeiculoService);

//   if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

//   const veiculoId = route.paramMap.get('id')!;

//   return veiculoService.selecionarPorId(veiculoId);
// };

// const listagemGruposVeiculosResolver: ResolveFn<ListagemGruposVeiculosModel[]> = () => {
//   const grupoVeiculoService = inject(GrupoVeiculoService);

//   return grupoVeiculoService.selecionarTodas();
// };

export const veiculoRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarVeiculos,
        resolve: { veiculos: listagemVeiculosResolver },
      },
      // {
      //   path: 'cadastrar',
      //   component: CadastrarVeiculo,
      //   resolve: { grupoVeiculos: listagemGruposVeiculosResolver },
      // },
      // {
      //   path: 'editar/:id',
      //   component: EditarVeiculo,
      //   resolve: { veiculo: detalhesVeiculoResolver, grupoVeiculos: listagemGruposVeiculosResolver },
      // },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirVeiculo,
      //   resolve: { veiculo: detalhesVeiculoResolver },
      // },
    ],
    providers: [VeiculoService],
  },
];
