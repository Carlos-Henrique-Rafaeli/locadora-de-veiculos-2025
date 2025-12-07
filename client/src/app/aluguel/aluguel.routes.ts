import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Routes } from '@angular/router';
import { ListagemAlugueisModel } from './aluguel.models';
import { AluguelService } from './aluguel.service';
import { ListarAlugueis } from './listar/listar-alugueis';
import { CondutorService } from '../condutor/condutor.service';
import { ListagemCondutoresModel } from '../condutor/condutor.models';
import { ListagemGruposVeiculosModel } from '../grupo-veiculo/grupoVeiculo.models';
import { GrupoVeiculoService } from '../grupo-veiculo/grupoVeiculo.service';
import { ListagemPlanosCobrancasModel } from '../plano-cobranca/planoCobranca.models';
import { PlanoCobrancaService } from '../plano-cobranca/planoCobranca.service';
import { ListagemTaxasServicosModel } from '../taxa-servico/taxaServico.models';
import { TaxaServicoService } from '../taxa-servico/taxaServico.service';
import { ListagemVeiculosModel } from '../veiculo/veiculo.models';
import { VeiculoService } from '../veiculo/veiculo.service';
import { CadastrarAluguel } from './cadastrar/cadastrar-aluguel';
import { EditarAluguel } from './editar/editar-aluguel';
import { FinalizarAluguel } from './finalizar/finalizar-aluguel';

const listagemAlugueisResolver: ResolveFn<ListagemAlugueisModel[]> = () => {
  const aluguelService = inject(AluguelService);

  return aluguelService.selecionarTodas();
};

const detalhesAluguelResolver = (route: ActivatedRouteSnapshot) => {
  const aluguelService = inject(AluguelService);

  if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

  const aluguelId = route.paramMap.get('id')!;

  return aluguelService.selecionarPorId(aluguelId);
};

const listagemCondutoresResolver: ResolveFn<ListagemCondutoresModel[]> = () => {
  const condutorService = inject(CondutorService);

  return condutorService.selecionarTodas();
};

const listagemGruposVeiculosResolver: ResolveFn<ListagemGruposVeiculosModel[]> = () => {
  const grupoVeiculoService = inject(GrupoVeiculoService);

  return grupoVeiculoService.selecionarTodas();
};

const listagemVeiculosResolver: ResolveFn<ListagemVeiculosModel[]> = () => {
  const veiculoService = inject(VeiculoService);

  return veiculoService.selecionarTodas();
};

const listagemPlanosCobrancasResolver: ResolveFn<ListagemPlanosCobrancasModel[]> = () => {
  const planoCobrancaService = inject(PlanoCobrancaService);

  return planoCobrancaService.selecionarTodas();
};

const listagemTaxasServicosResolver: ResolveFn<ListagemTaxasServicosModel[]> = () => {
  const taxaServicoService = inject(TaxaServicoService);

  return taxaServicoService.selecionarTodas();
};

export const aluguelRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarAlugueis,
        resolve: { alugueis: listagemAlugueisResolver },
      },
      {
        path: 'cadastrar',
        component: CadastrarAluguel,
        resolve: {
          condutores: listagemCondutoresResolver,
          gruposVeiculos: listagemGruposVeiculosResolver,
          veiculos: listagemVeiculosResolver,
          planosCobrancas: listagemPlanosCobrancasResolver,
          taxasServicos: listagemTaxasServicosResolver,
        },
      },
      {
        path: 'editar/:id',
        component: EditarAluguel,
        resolve: {
          aluguel: detalhesAluguelResolver,
          condutores: listagemCondutoresResolver,
          gruposVeiculos: listagemGruposVeiculosResolver,
          veiculos: listagemVeiculosResolver,
          planosCobrancas: listagemPlanosCobrancasResolver,
          taxasServicos: listagemTaxasServicosResolver,
        },
      },
      {
        path: 'finalizar/:id',
        component: FinalizarAluguel,
        resolve: { aluguel: detalhesAluguelResolver },
      },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirAluguel,
      //   resolve: { aluguel: detalhesAluguelResolver },
      // },
    ],
    providers: [
      AluguelService,
      CondutorService,
      GrupoVeiculoService,
      VeiculoService,
      PlanoCobrancaService,
      TaxaServicoService,
    ],
  },
];
