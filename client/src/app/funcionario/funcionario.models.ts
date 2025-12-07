export interface ListagemFuncionariosApiResponse {
  quantidadeRegistros: number;
  registros: ListagemFuncionariosModel[];
}

export interface ListagemFuncionariosModel {
  id: string;
  nomeCompleto: string;
  cpf: string;
  email: string;
  salario: number;
  admissaoEmUtc: Date;
}

export interface CadastrarFuncionarioModel {
  nomeCompleto: string;
  cpf: string;
  email: string;
  senha: string;
  confirmarSenha: string;
  salario: string;
  admissaoEmUtc: string;
}

export interface CadastrarFuncionarioResponseModel {
  id: string;
}

export interface EditarFuncionarioModel {
  nomeCompleto: string;
  cpf: string;
  salario: number;
  admissaoEmUtc: Date;
}

export interface EditarFuncionarioResponseModel {
  id: string;
}

export interface DetalhesFuncionarioModel {
  id: string;
  nomeCompleto: string;
  cpf: string;
  email: string;
  salario: number;
  admissaoEmUtc: Date;
}
