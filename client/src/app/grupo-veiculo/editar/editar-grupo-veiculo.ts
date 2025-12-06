import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { GrupoVeiculoService } from '../grupoVeiculo.service';
import {
  DetalhesGrupoVeiculoModel,
  EditarGrupoVeiculoModel,
  EditarGrupoVeiculoResponseModel,
} from '../grupoVeiculo.models';
import { Observer, take, switchMap, filter, map, shareReplay, tap } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-editar-grupo-veiculo',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    AsyncPipe,
    ReactiveFormsModule,
  ],
  templateUrl: './editar-grupo-veiculo.html',
})
export class EditarGrupoVeiculo {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly grupoVeiculoService = inject(GrupoVeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected grupoVeiculoForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
  });

  get nome() {
    return this.grupoVeiculoForm.get('nome');
  }

  protected readonly grupoVeiculo$ = this.route.data.pipe(
    filter((data) => data['grupoVeiculo']),
    map((data) => data['grupoVeiculo'] as DetalhesGrupoVeiculoModel),
    tap((grupoVeiculo) => this.grupoVeiculoForm.patchValue({ nome: grupoVeiculo.nome })),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.grupoVeiculoForm.invalid) return;

    const editarGrupoVeiculoModel: EditarGrupoVeiculoModel = this.grupoVeiculoForm.value;

    const edicaoObserver: Observer<EditarGrupoVeiculoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarGrupoVeiculoModel.nome}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/grupos-veiculos']),
    };

    this.grupoVeiculo$
      .pipe(
        take(1),
        switchMap((grupoVeiculo) =>
          this.grupoVeiculoService.editar(grupoVeiculo.id, editarGrupoVeiculoModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
