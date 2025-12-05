import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { filter, map } from 'rxjs';
import { ListagemCondutoresModel } from '../condutor.models';
import { CondutorService } from '../condutor.service';

@Component({
  selector: 'app-listar-condutores',
  imports: [MatButtonModule, MatIconModule, MatCardModule, RouterLink, AsyncPipe],
  templateUrl: './listar-condutores.html',
})
export class ListarCondutores {
  protected readonly route = inject(ActivatedRoute);
  protected readonly condutorService = inject(CondutorService);

  protected readonly condutores$ = this.route.data.pipe(
    filter((data) => data['condutores']),
    map((data) => data['condutores'] as ListagemCondutoresModel[]),
  );
}
