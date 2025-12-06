import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';
import { ListagemTaxasServicosModel } from './taxaServico.models';
import { TaxaServicoService } from './taxaServico.service';
import { ListarTaxasServicos } from './listar/listar-taxas-servicos';

const listagemTaxasServicosResolver: ResolveFn<ListagemTaxasServicosModel[]> = () => {
  const taxaServicoService = inject(TaxaServicoService);

  return taxaServicoService.selecionarTodas();
};

// const detalhesTaxasServicoResolver = (route: ActivatedRouteSnapshot) => {
//   const taxaServicoService = inject(TaxasServicoService);

//   if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

//   const taxaServicoId = route.paramMap.get('id')!;

//   return taxaServicoService.selecionarPorId(taxaServicoId);
// };

export const taxaServicoRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarTaxasServicos,
        resolve: { taxasServicos: listagemTaxasServicosResolver },
      },
      // { path: 'cadastrar', component: CadastrarTaxasServico },
      // {
      //   path: 'editar/:id',
      //   component: EditarTaxasServico,
      //   resolve: { taxaServico: detalhesTaxasServicoResolver },
      // },
      // {
      //   path: 'excluir/:id',
      //   component: ExcluirTaxasServico,
      //   resolve: { taxaServico: detalhesTaxasServicoResolver },
      // },
    ],
    providers: [TaxaServicoService],
  },
];
