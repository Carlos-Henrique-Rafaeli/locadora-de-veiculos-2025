import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { filter, map, shareReplay, Observer, take, switchMap } from 'rxjs';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { DetalhesTaxaServicoModel } from '../taxaServico.models';
import { TaxaServicoService } from '../taxaServico.service';

@Component({
  selector: 'app-excluir-taxa-servico',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    AsyncPipe,
    FormsModule,
  ],
  templateUrl: './excluir-taxa-servico.html',
})
export class ExcluirTaxaServico {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly taxaServicoService = inject(TaxaServicoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly taxaServico$ = this.route.data.pipe(
    filter((data) => data['taxaServico']),
    map((data) => data['taxaServico'] as DetalhesTaxaServicoModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public excluir() {
    const exclusaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi excluído com sucesso!`),
      error: (err) => {
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/taxas-servicos']),
    };

    this.taxaServico$
      .pipe(
        take(1),
        switchMap((taxaServico) => this.taxaServicoService.excluir(taxaServico.id)),
      )
      .subscribe(exclusaoObserver);
  }
}
