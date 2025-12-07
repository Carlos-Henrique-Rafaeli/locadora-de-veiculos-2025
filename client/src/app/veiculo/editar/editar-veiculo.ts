import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { filter, map, tap, shareReplay, Observer, switchMap, take } from 'rxjs';
import { ListagemGruposVeiculosModel } from '../../grupo-veiculo/grupoVeiculo.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  DetalhesVeiculoModel,
  EditarVeiculoModel,
  EditarVeiculoResponseModel,
} from '../veiculo.models';
import { VeiculoService } from '../veiculo.service';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { NgxMaskDirective } from 'ngx-mask';

@Component({
  selector: 'app-editar-veiculo',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    RouterLink,
    ReactiveFormsModule,
    AsyncPipe,
    NgxMaskDirective,
  ],
  templateUrl: './editar-veiculo.html',
})
export class EditarVeiculo {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly veiculoService = inject(VeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected veiculoForm: FormGroup = this.formBuilder.group({
    grupoVeiculoId: ['', [Validators.required]],
    placa: [
      '',
      [Validators.required, Validators.pattern(/^(?:[A-z]{3}-\d{4}|[A-Z]{3}\d[A-z]\d{2})$/)],
    ],
    modelo: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    marca: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    cor: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    tipoCombustivel: ['', [Validators.required]],
    capacidadeTanque: ['', [Validators.required]],
  });

  get grupoVeiculoId() {
    return this.veiculoForm.get('grupoVeiculoId');
  }

  get placa() {
    return this.veiculoForm.get('placa');
  }

  get modelo() {
    return this.veiculoForm.get('modelo');
  }

  get marca() {
    return this.veiculoForm.get('marca');
  }

  get cor() {
    return this.veiculoForm.get('cor');
  }

  get tipoCombustivel() {
    return this.veiculoForm.get('tipoCombustivel');
  }

  get capacidadeTanque() {
    return this.veiculoForm.get('capacidadeTanque');
  }

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as ListagemGruposVeiculosModel[]),
  );

  protected readonly tiposCombustiveis = ['Gasolina', 'Etanol', 'Diesel'];

  protected readonly veiculo$ = this.route.data.pipe(
    filter((data) => data['veiculo']),
    map((data) => data['veiculo'] as DetalhesVeiculoModel),
    tap((veiculo) => {
      console.log(veiculo);

      this.veiculoForm.patchValue({
        grupoVeiculoId: veiculo.grupoVeiculo.id,
        placa: veiculo.placa,
        modelo: veiculo.modelo,
        marca: veiculo.marca,
        cor: veiculo.cor,
        tipoCombustivel: veiculo.tipoCombustivel,
        capacidadeTanque: veiculo.capacidadeTanque,
      });
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.veiculoForm.invalid) return;

    const editarVeiculoModel: EditarVeiculoModel = this.veiculoForm.value;

    editarVeiculoModel.placa = editarVeiculoModel.placa.toUpperCase();

    const edicaoObserver: Observer<EditarVeiculoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O registro "${editarVeiculoModel.placa}" foi editado com sucesso!`,
        ),
      error: (err) => {
        console.log(err.error);
        const msg = err.error[1] || err.error[0] || 'Erro desconhecido.';

        this.notificacaoService.erro(msg);
      },
      complete: () => this.router.navigate(['/veiculos']),
    };

    this.veiculo$
      .pipe(
        take(1),
        switchMap((veiculo) => this.veiculoService.editar(veiculo.id, editarVeiculoModel)),
      )
      .subscribe(edicaoObserver);
  }
}
