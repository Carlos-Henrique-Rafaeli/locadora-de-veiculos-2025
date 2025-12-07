import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarVeiculoModel,
  CadastrarVeiculoResponseModel,
  EditarVeiculoModel,
  EditarVeiculoResponseModel,
  DetalhesVeiculoModel,
  ListagemVeiculosApiResponse,
  ListagemVeiculosModel,
} from './veiculo.models';

@Injectable()
export class VeiculoService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/veiculos';

  public cadastrar(model: CadastrarVeiculoModel): Observable<CadastrarVeiculoResponseModel> {
    const formData = new FormData();

    Object.entries(model).forEach(([key, value]) => {
      if (value === null || value === undefined) return;

      if (value instanceof File) {
        formData.append(key, value);
      } else {
        formData.append(key, String(value));
      }
    });

    return this.http.post<CadastrarVeiculoResponseModel>(this.apiUrl, formData);
  }

  public editar(
    id: string,
    editarVeiculoModel: EditarVeiculoModel,
  ): Observable<EditarVeiculoResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    const formData = new FormData();

    Object.entries(editarVeiculoModel).forEach(([key, value]) => {
      if (value === null || value === undefined) return;

      if (value instanceof File) {
        formData.append(key, value);
      } else {
        formData.append(key, String(value));
      }
    });

    return this.http.put<EditarVeiculoResponseModel>(urlCompleto, formData);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemVeiculosModel[]> {
    return this.http
      .get<ListagemVeiculosApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesVeiculoModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ veiculo: DetalhesVeiculoModel }>(urlCompleto)
      .pipe(map((res) => res.veiculo));
  }
}
