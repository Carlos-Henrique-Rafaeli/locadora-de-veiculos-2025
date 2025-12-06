import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs';
import { ListagemTaxasServicosModel } from '../taxaServico.models';
import { TaxaServicoService } from '../taxaServico.service';

@Component({
  selector: 'app-listar-taxas-servicos',
  imports: [MatButtonModule, MatIconModule, MatCardModule, RouterLink, AsyncPipe, CurrencyPipe],
  templateUrl: './listar-taxas-servicos.html',
})
export class ListarTaxasServicos {
  protected readonly route = inject(ActivatedRoute);
  protected readonly taxaServicoService = inject(TaxaServicoService);

  protected readonly taxasServicos$ = this.route.data.pipe(
    filter((data) => data['taxasServicos']),
    map((data) => {
      const taxasServicos = data['taxasServicos'] as ListagemTaxasServicosModel[];

      return taxasServicos.map((p) => ({
        ...p,
        tipoCobranca: p.tipoCobranca === 'PrecoFixo' ? 'Preço Fixo' : 'Cobrança Diária',
      }));
    }),
  );
}
