import { ListagemCondutoresModel } from '../condutor/condutor.models';
import { ListagemPlanosCobrancasModel } from '../plano-cobranca/planoCobranca.models';
import { ListagemTaxasServicosModel } from '../taxa-servico/taxaServico.models';
import {
  ListagemGrupoVeiculoModelSimplified,
  ListagemVeiculosModel,
} from '../veiculo/veiculo.models';

export interface ListagemAlugueisApiResponse {
  quantidadeRegistros: number;
  registros: ListagemAlugueisModel[];
}

export interface ListagemAlugueisModel {
  id: string;
  condutor: ListagemCondutoresModel;
  grupoVeiculo: ListagemGrupoVeiculoModelSimplified;
  veiculo: ListagemVeiculosModel;
  dataInicio: Date;
  dataFim: Date;
  planoCobranca: ListagemPlanosCobrancasModel;
  taxasServicos: ListagemTaxasServicosModel[];
  valorTotal: number;
  estaAberta: boolean;
}

export interface CadastrarAluguelModel {
  condutorId: string;
  grupoVeiculoId: string;
  veiculoId: string;
  dataEntrada: Date;
  dataRetorno: Date;
  planoCobrancaId: string;
  taxasServicosIds: string[];
}

export interface CadastrarAluguelResponseModel {
  id: string;
}

export interface EditarAluguelModel {
  condutorId: string;
  grupoVeiculoId: string;
  veiculoId: string;
  dataEntrada: Date;
  dataRetorno: Date;
  planoCobrancaId: string;
  taxasServicosIds: string[];
}

export interface EditarAluguelResponseModel {
  id: string;
}

export interface FinalizarAluguelModel {
  dataRetorno: Date;
  kmInicial: number;
  kmAtual: number;
  tanqueCheio: boolean;
  porcentagemTanque: number | undefined;
}

export interface FinalizarAluguelResponseModel {
  id: string;
  valorFinal: number;
}

export interface DetalhesAluguelModel {
  id: string;
  condutor: ListagemCondutoresModel;
  grupoVeiculo: ListagemGrupoVeiculoModelSimplified;
  veiculo: ListagemVeiculosModel;
  dataInicio: Date;
  dataFim: Date;
  planoCobranca: ListagemPlanosCobrancasModel;
  taxasServicos: ListagemTaxasServicosModel[];
  valorTotal: number;
  estaAberta: boolean;
}
