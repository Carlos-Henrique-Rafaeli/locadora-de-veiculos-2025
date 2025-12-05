export interface ListagemClientesApiResponse {
  quantidadeRegistros: number;
  registros: ListagemClientesModel[];
}

export interface ListagemClientesModel {
  id: string;
  tipoCliente: string;
  nome: string;
  telefone: string;
  cpf: string;
  cnpj: string;
  estado: string;
  cidade: string;
  bairro: string;
  rua: string;
  numero: number;
}

export interface CadastrarClienteModel {
  tipoCliente: string;
  nome: string;
  telefone: string;
  cpf: string;
  cnpj: string;
  estado: string;
  cidade: string;
  bairro: string;
  rua: string;
  numero: number;
}

export interface CadastrarClienteResponseModel {
  id: string;
}
