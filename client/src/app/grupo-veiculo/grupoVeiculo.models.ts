export interface ListagemGruposVeiculosApiResponse {
  quantidadeRegistros: number;
  registros: ListagemGruposVeiculosModel[];
}

export interface ListagemGruposVeiculosModel {
  id: string;
  nome: string;
  veiculos: ListagemVeiculosModelSimplified[];
}

export interface ListagemVeiculosModelSimplified {
  id: string;
  placa: string;
}

export interface CadastrarGrupoVeiculoModel {
  nome: string;
}

export interface CadastrarGrupoVeiculoResponseModel {
  id: string;
}

export interface EditarGrupoVeiculoModel {
  nome: string;
}

export interface EditarGrupoVeiculoResponseModel {
  id: string;
}

export interface DetalhesGrupoVeiculoModel {
  id: string;
  nome: string;
  veiculos: ListagemVeiculosModelSimplified;
}
