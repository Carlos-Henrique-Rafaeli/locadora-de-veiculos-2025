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
import { DetalhesPlanoCobrancaModel } from '../planoCobranca.models';
import { PlanoCobrancaService } from '../planoCobranca.service';

@Component({
  selector: 'app-excluir-plano-cobranca',
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
  templateUrl: './excluir-plano-cobranca.html',
})
export class ExcluirPlanoCobranca {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly planoCobranca$ = this.route.data.pipe(
    filter((data) => data['planoCobranca']),
    map((data) => data['planoCobranca'] as DetalhesPlanoCobrancaModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public excluir() {
    const exclusaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi excluído com sucesso!`),
      error: (err) => {
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/planos-cobrancas']),
    };

    this.planoCobranca$
      .pipe(
        take(1),
        switchMap((planoCobranca) => this.planoCobrancaService.excluir(planoCobranca.id)),
      )
      .subscribe(exclusaoObserver);
  }
}
