import { AsyncPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs';
import { AluguelService } from '../aluguel.service';
import { ListagemAlugueisModel } from '../aluguel.models';

@Component({
  selector: 'app-listar-alugueis',
  imports: [
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    RouterLink,
    AsyncPipe,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './listar-alugueis.html',
})
export class ListarAlugueis {
  protected readonly route = inject(ActivatedRoute);
  protected readonly aluguelService = inject(AluguelService);

  protected readonly alugueis$ = this.route.data.pipe(
    filter((data) => data['alugueis']),
    map((data) => {
      const alugueis = data['alugueis'] as ListagemAlugueisModel[];

      return alugueis.map((p) => ({
        ...p,
        planoCobranca: {
          id: p.planoCobranca.id,
          tipoPlano: this.converterTipo(p.planoCobranca.tipoPlano),
          grupoVeiculo: {
            id: p.planoCobranca.grupoVeiculo.id,
            nome: p.planoCobranca.grupoVeiculo.nome,
          },
          valorDiario: p.planoCobranca.valorDiario,
          valorKm: p.planoCobranca.valorKm,
          kmIncluso: p.planoCobranca.kmIncluso,
          valorFixo: p.planoCobranca.valorDiario,
        },
      }));
    }),
  );

  private converterTipo(plano: string) {
    if (plano === 'PlanoDiario') return 'Plano Diário';
    else if (plano === 'PlanoControlado') return 'Plano Controlado';
    else return 'Plano Livre';
  }
}
