import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { filter, map } from 'rxjs';
import { ListagemPrecoCombustivelModel } from '../precoCombustivel.model';
import { PrecoCombustivelService } from '../precoCombustivel.service';

@Component({
  selector: 'app-listar-preco-combustivel',
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
    CurrencyPipe,
  ],
  templateUrl: './listar-preco-combustivel.html',
})
export class ListarPrecoCombustivel {
  protected readonly route = inject(ActivatedRoute);
  protected readonly precoCombustivelService = inject(PrecoCombustivelService);

  protected readonly precoCombustivel$ = this.route.data.pipe(
    filter((data) => data['precoCombustivel']),
    map((data) => data['precoCombustivel'] as ListagemPrecoCombustivelModel),
  );
}
