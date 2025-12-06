import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarTaxaServicoModel,
  CadastrarTaxaServicoResponseModel,
  EditarTaxaServicoModel,
  EditarTaxaServicoResponseModel,
  ListagemTaxasServicosModel,
  ListagemTaxasServicosApiResponse,
  DetalhesTaxaServicoModel,
} from './taxaServico.models';

@Injectable()
export class TaxaServicoService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/taxa-servico';

  public cadastrar(
    medicoModel: CadastrarTaxaServicoModel,
  ): Observable<CadastrarTaxaServicoResponseModel> {
    return this.http.post<CadastrarTaxaServicoResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarTaxaServicoModel: EditarTaxaServicoModel,
  ): Observable<EditarTaxaServicoResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarTaxaServicoResponseModel>(urlCompleto, editarTaxaServicoModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemTaxasServicosModel[]> {
    return this.http
      .get<ListagemTaxasServicosApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesTaxaServicoModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ taxaServicoDto: DetalhesTaxaServicoModel }>(urlCompleto)
      .pipe(map((res) => res.taxaServicoDto));
  }
}
