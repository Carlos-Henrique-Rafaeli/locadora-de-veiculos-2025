import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Routes } from '@angular/router';
import { ListarPlanosCobrancas } from './listar/listar-planos-cobrancas';
import { ListagemPlanosCobrancasModel } from './planoCobranca.models';
import { PlanoCobrancaService } from './planoCobranca.service';
import { ListagemGruposVeiculosModel } from '../grupo-veiculo/grupoVeiculo.models';
import { GrupoVeiculoService } from '../grupo-veiculo/grupoVeiculo.service';
import { CadastrarPlanoCobranca } from './cadastrar/cadastrar-plano-cobranca';
import { EditarPlanoCobranca } from './editar/editar-plano-cobranca';

const listagemPlanosCobrancasResolver: ResolveFn<ListagemPlanosCobrancasModel[]> = () => {
  const planosCobrancaService = inject(PlanoCobrancaService);

  return planosCobrancaService.selecionarTodas();
};

const detalhesPlanoCobrancaResolver = (route: ActivatedRouteSnapshot) => {
  const planosCobrancaService = inject(PlanoCobrancaService);

  if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

  const planosCobrancaId = route.paramMap.get('id')!;

  return planosCobrancaService.selecionarPorId(planosCobrancaId);
};

const listagemGruposVeiculosResolver: ResolveFn<ListagemGruposVeiculosModel[]> = () => {
  const grupoVeiculoService = inject(GrupoVeiculoService);

  return grupoVeiculoService.selecionarTodas();
};

export const planoCobrancaRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarPlanosCobrancas,
        resolve: { planosCobrancas: listagemPlanosCobrancasResolver },
      },
      {
        path: 'cadastrar',
        component: CadastrarPlanoCobranca,
        resolve: { gruposVeiculos: listagemGruposVeiculosResolver },
      },
      {
        path: 'editar/:id',
        component: EditarPlanoCobranca,
        resolve: {
          planoCobranca: detalhesPlanoCobrancaResolver,
          gruposVeiculos: listagemGruposVeiculosResolver,
        },
      },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirPlanoCobranca,
      //   resolve: { gruposVeiculos: detalhesPlanoCobrancaResolver },
      // },
    ],
    providers: [PlanoCobrancaService, GrupoVeiculoService],
  },
];
