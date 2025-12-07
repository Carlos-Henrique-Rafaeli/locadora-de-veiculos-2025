import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  CadastrarFuncionarioModel,
  CadastrarFuncionarioResponseModel,
  EditarFuncionarioModel,
  EditarFuncionarioResponseModel,
  ListagemFuncionariosModel,
  ListagemFuncionariosApiResponse,
  DetalhesFuncionarioModel,
} from './funcionario.models';

@Injectable()
export class FuncionarioService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = environment.apiUrl + '/funcionarios';

  public cadastrar(
    medicoModel: CadastrarFuncionarioModel,
  ): Observable<CadastrarFuncionarioResponseModel> {
    return this.http.post<CadastrarFuncionarioResponseModel>(this.apiUrl, medicoModel);
  }

  public editar(
    id: string,
    editarFuncionarioModel: EditarFuncionarioModel,
  ): Observable<EditarFuncionarioResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;
    return this.http.put<EditarFuncionarioResponseModel>(urlCompleto, editarFuncionarioModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarTodas(): Observable<ListagemFuncionariosModel[]> {
    return this.http
      .get<ListagemFuncionariosApiResponse>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarPorId(id: string): Observable<DetalhesFuncionarioModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http
      .get<{ funcionario: DetalhesFuncionarioModel }>(urlCompleto)
      .pipe(map((res) => res.funcionario));
  }
}
