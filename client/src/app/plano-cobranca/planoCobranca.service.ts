import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarPlanoCobrancaModel,
  CadastrarPlanoCobrancaResponseModel,
  EditarPlanoCobrancaModel,
  EditarPlanoCobrancaResponseModel,
  ListagemPlanosCobrancasModel,
  ListagemPlanosCobrancasApiResponse,
  DetalhesPlanoCobrancaModel,
} from './planoCobranca.models';

@Injectable()
export class PlanoCobrancaService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/plano-cobranca';

  public cadastrar(
    medicoModel: CadastrarPlanoCobrancaModel,
  ): Observable<CadastrarPlanoCobrancaResponseModel> {
    return this.http.post<CadastrarPlanoCobrancaResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarPlanoCobrancaModel: EditarPlanoCobrancaModel,
  ): Observable<EditarPlanoCobrancaResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarPlanoCobrancaResponseModel>(urlCompleto, editarPlanoCobrancaModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemPlanosCobrancasModel[]> {
    return this.http
      .get<ListagemPlanosCobrancasApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesPlanoCobrancaModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ planoCobrancaDto: DetalhesPlanoCobrancaModel }>(urlCompleto)
      .pipe(map((res) => res.planoCobrancaDto));
  }
}
