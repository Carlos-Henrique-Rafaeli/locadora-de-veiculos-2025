import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs';
import { ListagemPlanosCobrancasModel } from '../planoCobranca.models';
import { PlanoCobrancaService } from '../planoCobranca.service';

@Component({
  selector: 'app-listar-planos-cobrancas',
  imports: [MatButtonModule, MatIconModule, MatCardModule, RouterLink, AsyncPipe],
  templateUrl: './listar-planos-cobrancas.html',
})
export class ListarPlanosCobrancas {
  protected readonly route = inject(ActivatedRoute);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);

  protected readonly planosCobrancas$ = this.route.data.pipe(
    filter((data) => data['planosCobrancas']),
    map((data) => {
      const planos = data['planosCobrancas'] as ListagemPlanosCobrancasModel[];

      return planos.map((p) => ({
        ...p,
        tipoPlano: this.converterTipo(p.tipoPlano),
      }));
    }),
  );

  private converterTipo(plano: string) {
    if (plano === 'PlanoDiario') return 'Plano Diário';
    else if (plano === 'PlanoControlado') return 'Plano Controlado';
    else return 'Plano Livre';
  }
}
