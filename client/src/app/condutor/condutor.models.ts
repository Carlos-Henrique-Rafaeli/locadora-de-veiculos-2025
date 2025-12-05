export interface ListagemCondutoresApiResponse {
  quantidadeRegistros: number;
  registros: ListagemCondutoresModel[];
}

export interface ListagemCondutoresModel {
  id: string;
  cliente: ListagemClientesModelSimplified;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  cpf: string;
  cnh: string;
  validadeCnh: Date;
  telefone: string;
}

export interface ListagemClientesModelSimplified {
  id: string;
  tipoCliente: string;
  nome: string;
  telefone: string;
  cpf: string | null;
  cnpj: string | null;
}

export interface CadastrarCondutorModel {
  clienteId: string;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  cpf: string;
  cnh: string;
  validadeCnh: Date;
  telefone: string;
}

export interface CadastrarCondutorResponseModel {
  id: string;
}

export interface EditarCondutorModel {
  clienteId: string;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  cpf: string;
  cnh: string;
  validadeCnh: Date;
  telefone: string;
}

export interface EditarCondutorResponseModel {
  id: string;
}

export interface DetalhesCondutorModel {
  id: string;
  cliente: ListagemClientesModelSimplified;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  cpf: string;
  cnh: string;
  validadeCnh: Date;
  telefone: string;
}
