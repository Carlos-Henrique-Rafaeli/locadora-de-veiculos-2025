import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarGrupoVeiculoModel,
  CadastrarGrupoVeiculoResponseModel,
  EditarGrupoVeiculoModel,
  EditarGrupoVeiculoResponseModel,
  ListagemGruposVeiculosModel,
  ListagemGruposVeiculosApiResponse,
  DetalhesGrupoVeiculoModel,
} from './grupoVeiculo.models';

@Injectable()
export class GrupoVeiculoService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/grupo-veiculo';

  public cadastrar(
    medicoModel: CadastrarGrupoVeiculoModel,
  ): Observable<CadastrarGrupoVeiculoResponseModel> {
    return this.http.post<CadastrarGrupoVeiculoResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarGrupoVeiculoModel: EditarGrupoVeiculoModel,
  ): Observable<EditarGrupoVeiculoResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarGrupoVeiculoResponseModel>(urlCompleto, editarGrupoVeiculoModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemGruposVeiculosModel[]> {
    return this.http
      .get<ListagemGruposVeiculosApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesGrupoVeiculoModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ grupoVeiculo: DetalhesGrupoVeiculoModel }>(urlCompleto)
      .pipe(map((res) => res.grupoVeiculo));
  }
}
