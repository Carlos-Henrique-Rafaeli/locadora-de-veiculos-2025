import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  ListagemClientesModel,
  ListagemClientesApiResponse,
  CadastrarClienteModel,
  CadastrarClienteResponseModel,
} from './cliente.models';

@Injectable()
export class ClienteService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/cliente';

  public cadastrar(medicoModel: CadastrarClienteModel): Observable<CadastrarClienteResponseModel> {
    return this.http.post<CadastrarClienteResponseModel>(this.apiUrl, medicoModel);
  }

  public selecionarTodas(): Observable<ListagemClientesModel[]> {
    return this.http
      .get<ListagemClientesApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
