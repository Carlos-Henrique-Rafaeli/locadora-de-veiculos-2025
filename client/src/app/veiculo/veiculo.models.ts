export interface ListagemVeiculosApiResponse {
  quantidadeRegistros: number;
  registros: ListagemVeiculosModel[];
}

export interface ListagemVeiculosModel {
  id: string;
  grupoVeiculo: ListagemGrupoVeiculoModelSimplified;
  placa: string;
  modelo: string;
  marca: string;
  cor: string;
  tipoCombustivel: string;
  capacidadeTanque: number;
}

export interface ListagemGrupoVeiculoModelSimplified {
  id: string;
  nome: string;
}

export interface CadastrarVeiculoModel {
  grupoVeiculoid: string;
  placa: string;
  modelo: string;
  marca: string;
  cor: string;
  tipoCombustivel: string;
  capacidadeTanque: number;
}

export interface CadastrarVeiculoResponseModel {
  id: string;
}

export interface EditarVeiculoModel {
  grupoVeiculoid: string;
  placa: string;
  modelo: string;
  marca: string;
  cor: string;
  tipoCombustivel: string;
  capacidadeTanque: number;
}

export interface EditarVeiculoResponseModel {
  id: string;
}

export interface DetalhesVeiculoModel {
  id: string;
  grupoVeiculo: ListagemGrupoVeiculoModelSimplified;
  placa: string;
  modelo: string;
  marca: string;
  cor: string;
  tipoCombustivel: string;
  capacidadeTanque: number;
}
