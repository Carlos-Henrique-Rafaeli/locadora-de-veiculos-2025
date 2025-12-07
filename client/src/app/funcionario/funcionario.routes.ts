import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, Routes } from '@angular/router';
import { ListagemFuncionariosModel } from './funcionario.models';
import { FuncionarioService } from './funcionario.service';
import { ListarFuncionarios } from './listar/listar-funcionarios';
import { CadastrarFuncionario } from './cadastrar/cadastrar-funcionario';
import { provideNgxMask } from 'ngx-mask';
import { EditarFuncionario } from './editar/editar-funcionario';
import { ExcluirFuncionario } from './excluir/excluir-funcionario';

const listagemFuncionariosResolver: ResolveFn<ListagemFuncionariosModel[]> = () => {
  const funcionarioService = inject(FuncionarioService);

  return funcionarioService.selecionarTodas();
};

const detalhesFuncionarioResolver = (route: ActivatedRouteSnapshot) => {
  const funcionarioService = inject(FuncionarioService);

  if (!route.paramMap.has('id')) throw new Error('O parâmetro id não foi fornecido.');

  const funcionarioId = route.paramMap.get('id')!;

  return funcionarioService.selecionarPorId(funcionarioId);
};

export const funcionarioRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarFuncionarios,
        resolve: { funcionarios: listagemFuncionariosResolver },
      },
      { path: 'cadastrar', component: CadastrarFuncionario },
      {
        path: 'editar/:id',
        component: EditarFuncionario,
        resolve: { funcionario: detalhesFuncionarioResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirFuncionario,
        resolve: { funcionario: detalhesFuncionarioResolver },
      },
    ],
    providers: [FuncionarioService, provideNgxMask()],
  },
];
