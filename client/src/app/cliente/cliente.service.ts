import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  ListagemClientesModel,
  ListagemClientesApiResponse,
  CadastrarClienteModel,
  CadastrarClienteResponseModel,
  EditarClienteModel,
  EditarClienteResponseModel,
  DetalhesClienteModel,
} from './cliente.models';

@Injectable()
export class ClienteService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/cliente';

  public cadastrar(medicoModel: CadastrarClienteModel): Observable<CadastrarClienteResponseModel> {
    return this.http.post<CadastrarClienteResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarClienteModel: EditarClienteModel,
  ): Observable<EditarClienteResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarClienteResponseModel>(urlCompleto, editarClienteModel);
  }

  public selecionarTodas(): Observable<ListagemClientesModel[]> {
    return this.http
      .get<ListagemClientesApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesClienteModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ cliente: DetalhesClienteModel }>(urlCompleto)
      .pipe(map((res) => res.cliente));
  }
}
