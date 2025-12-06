import { ListagemGrupoVeiculoModelSimplified } from '../veiculo/veiculo.models';

export interface ListagemPlanosCobrancasApiResponse {
  quantidadeRegistros: number;
  registros: ListagemPlanosCobrancasModel[];
}

export interface ListagemPlanosCobrancasModel {
  id: string;
  tipoPlano: string;
  grupoVeiculo: ListagemGrupoVeiculoModelSimplified;
  valorDiario: number | null;
  valorKm: number | null;
  kmIncluso: number | null;
  valorKmExcedente: number | null;
  valorFixo: number | null;
}

export interface CadastrarPlanoCobrancaModel {
  tipoPlano: string;
  grupoVeiculoId: string;
  telefone: string;
  valorDiario: number | undefined;
  valorKm: number | undefined;
  kmIncluso: number | undefined;
  valorKmExcedente: number | undefined;
  valorFixo: number | undefined;
}

export interface CadastrarPlanoCobrancaResponseModel {
  id: string;
}

export interface EditarPlanoCobrancaModel {
  tipoPlano: string;
  grupoVeiculoId: string;
  telefone: string;
  valorDiario: number | undefined;
  valorKm: number | undefined;
  kmIncluso: number | undefined;
  valorKmExcedente: number | undefined;
  valorFixo: number | undefined;
}

export interface EditarPlanoCobrancaResponseModel {
  id: string;
}

export interface DetalhesPlanoCobrancaModel {
  id: string;
  tipoPlano: string;
  grupoVeiculo: ListagemGrupoVeiculoModelSimplified;
  valorDiario: number | null;
  valorKm: number | null;
  kmIncluso: number | null;
  valorKmExcedente: number | null;
  valorFixo: number | null;
}
