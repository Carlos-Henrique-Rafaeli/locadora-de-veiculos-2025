import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { filter, map } from 'rxjs';
import { ListagemVeiculosModel } from '../veiculo.models';
import { VeiculoService } from '../veiculo.service';

@Component({
  selector: 'app-listar-veiculos',
  imports: [MatButtonModule, MatIconModule, MatCardModule, RouterLink, AsyncPipe],
  templateUrl: './listar-veiculos.html',
})
export class ListarVeiculos {
  protected readonly route = inject(ActivatedRoute);
  protected readonly veiculoService = inject(VeiculoService);

  protected readonly veiculos$ = this.route.data.pipe(
    filter((data) => data['veiculos']),
    map((data) => data['veiculos'] as ListagemVeiculosModel[]),
  );
}
