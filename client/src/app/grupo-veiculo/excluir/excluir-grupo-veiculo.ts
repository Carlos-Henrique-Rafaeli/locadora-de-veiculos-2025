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
import { DetalhesGrupoVeiculoModel } from '../grupoVeiculo.models';
import { GrupoVeiculoService } from '../grupoVeiculo.service';

@Component({
  selector: 'app-excluir-grupo-veiculo',
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
  templateUrl: './excluir-grupo-veiculo.html',
})
export class ExcluirGrupoVeiculo {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly grupoVeiculoService = inject(GrupoVeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly grupoVeiculo$ = this.route.data.pipe(
    filter((data) => data['grupoVeiculo']),
    map((data) => data['grupoVeiculo'] as DetalhesGrupoVeiculoModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public excluir() {
    const exclusaoObserver: Observer<null> = {
      next: () => this.notificacaoService.sucesso(`O registro foi excluído com sucesso!`),
      error: (err) => {
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/grupos-veiculos']),
    };

    this.grupoVeiculo$
      .pipe(
        take(1),
        switchMap((grupoVeiculo) => this.grupoVeiculoService.excluir(grupoVeiculo.id)),
      )
      .subscribe(exclusaoObserver);
  }
}
