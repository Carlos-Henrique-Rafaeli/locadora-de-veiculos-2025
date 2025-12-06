export interface ListagemTaxasServicosApiResponse {
  quantidadeRegistros: number;
  registros: ListagemTaxasServicosModel[];
}

export interface ListagemTaxasServicosModel {
  id: string;
  nome: string;
  valor: number;
  tipoCobranca: string;
}

export interface CadastrarTaxaServicoModel {
  nome: string;
  valor: number;
  tipoCobranca: string;
}

export interface CadastrarTaxaServicoResponseModel {
  id: string;
}

export interface EditarTaxaServicoModel {
  nome: string;
  valor: number;
  tipoCobranca: string;
}

export interface EditarTaxaServicoResponseModel {
  id: string;
}

export interface DetalhesTaxaServicoModel {
  id: string;
  nome: string;
  valor: number;
  tipoCobranca: string;
}
