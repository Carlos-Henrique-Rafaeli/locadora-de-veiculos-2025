import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarAluguelModel,
  CadastrarAluguelResponseModel,
  EditarAluguelModel,
  EditarAluguelResponseModel,
  ListagemAlugueisModel,
  ListagemAlugueisApiResponse,
  DetalhesAluguelModel,
  FinalizarAluguelModel,
  FinalizarAluguelResponseModel,
} from './aluguel.models';

@Injectable()
export class AluguelService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/aluguel';

  public cadastrar(medicoModel: CadastrarAluguelModel): Observable<CadastrarAluguelResponseModel> {
    return this.http.post<CadastrarAluguelResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarAluguelModel: EditarAluguelModel,
  ): Observable<EditarAluguelResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarAluguelResponseModel>(urlCompleto, editarAluguelModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemAlugueisModel[]> {
    return this.http
      .get<ListagemAlugueisApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesAluguelModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ aluguel: DetalhesAluguelModel }>(urlCompleto)
      .pipe(map((res) => res.aluguel));
  }

  public finalizar(
    id: string,
    finalizarAluguelModel: FinalizarAluguelModel,
  ): Observable<FinalizarAluguelResponseModel> {
    const urlCompleto = `${this.apiUrl}/finalizar/${id}`;
    return this.http.post<FinalizarAluguelResponseModel>(urlCompleto, finalizarAluguelModel);
  }
}
