import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarCondutorModel,
  CadastrarCondutorResponseModel,
  EditarCondutorModel,
  EditarCondutorResponseModel,
  ListagemCondutoresModel,
  ListagemCondutoresApiResponse,
  DetalhesCondutorModel,
} from './condutor.models';

@Injectable()
export class CondutorService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/condutor';

  public cadastrar(
    medicoModel: CadastrarCondutorModel,
  ): Observable<CadastrarCondutorResponseModel> {
    return this.http.post<CadastrarCondutorResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarCondutorModel: EditarCondutorModel,
  ): Observable<EditarCondutorResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarCondutorResponseModel>(urlCompleto, editarCondutorModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemCondutoresModel[]> {
    return this.http
      .get<ListagemCondutoresApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesCondutorModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ condutor: DetalhesCondutorModel }>(urlCompleto)
      .pipe(map((res) => res.condutor));
  }
}
