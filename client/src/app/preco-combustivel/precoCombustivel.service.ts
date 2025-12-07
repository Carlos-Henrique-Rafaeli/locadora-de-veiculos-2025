import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  EditarPrecoCombustivelModel,
  ListagemPrecoCombustivelModel,
} from './precoCombustivel.model';

@Injectable()
export class PrecoCombustivelService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/configuracoes';

  public editar(editarPrecoCombustivelModel: EditarPrecoCombustivelModel): Observable<null> {
    return this.http.put<null>(this.apiUrl, editarPrecoCombustivelModel);
  }

  public selecionar(): Observable<ListagemPrecoCombustivelModel> {
    return this.http.get<ListagemPrecoCombustivelModel>(this.apiUrl);
  }
}
