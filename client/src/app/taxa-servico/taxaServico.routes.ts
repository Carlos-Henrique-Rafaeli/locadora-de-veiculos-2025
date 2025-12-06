import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Routes } from '@angular/router';
import { ListagemTaxasServicosModel } from './taxaServico.models';
import { TaxaServicoService } from './taxaServico.service';
import { ListarTaxasServicos } from './listar/listar-taxas-servicos';
import { CadastrarTaxaServico } from './cadastrar/cadastrar-taxa-servico';
import { EditarTaxaServico } from './editar/editar-taxa-servico';

const listagemTaxasServicosResolver: ResolveFn<ListagemTaxasServicosModel[]> = () => {
  const taxaServicoService = inject(TaxaServicoService);

  return taxaServicoService.selecionarTodas();
};

const detalhesTaxaServicoResolver = (route: ActivatedRouteSnapshot) => {
  const taxaServicoService = inject(TaxaServicoService);

  if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

  const taxaServicoId = route.paramMap.get('id')!;

  return taxaServicoService.selecionarPorId(taxaServicoId);
};

export const taxaServicoRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarTaxasServicos,
        resolve: { taxasServicos: listagemTaxasServicosResolver },
      },
      { path: 'cadastrar', component: CadastrarTaxaServico },
      {
        path: 'editar/:id',
        component: EditarTaxaServico,
        resolve: { taxaServico: detalhesTaxaServicoResolver },
      },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirTaxasServico,
      //   resolve: { taxaServico: detalhesTaxasServicoResolver },
      // },
    ],
    providers: [TaxaServicoService],
  },
];
