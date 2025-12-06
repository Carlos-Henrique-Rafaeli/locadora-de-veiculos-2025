import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe } from '@angular/common';
import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Observable, map, shareReplay } from 'rxjs';
import { UsuarioAutenticadoModel } from '../../auth/auth.models';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatMenuModule,
    AsyncPipe,
    RouterLink,
    RouterLinkActive,
  ],
})
export class ShellComponent {
  private readonly breakpointObserver = inject(BreakpointObserver);

  public isHandset$: Observable<boolean> = this.breakpointObserver
    .observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Handset])
    .pipe(
      map((result) => result.matches),
      shareReplay(),
    );

  public itensNavbar = [
    { titulo: 'Início', icone: 'home', link: '/inicio' },
    { titulo: 'Cliente', icone: 'person_add', link: '/clientes' },
    { titulo: 'Condutor', icone: 'assignment_ind', link: '/condutores' },
    { titulo: 'Grupo Veículo', icone: 'garage', link: '/grupos-veiculos' },
    { titulo: 'Veículo', icone: 'directions_car', link: '/veiculos' },
    { titulo: 'Plano de Cobrança', icone: 'payments', link: '/planos-cobrancas' },
    { titulo: 'Taxa/Serviço', icone: 'home_repair_service', link: '/taxas-servicos' },
    { titulo: 'Aluguel', icone: 'car_rental', link: '/alugueis' },
    { titulo: 'Preço Combustível', icone: 'local_gas_station', link: '/configuracao-preco' },
  ];

  @Input({ required: true }) usuarioAutenticado?: UsuarioAutenticadoModel;
  @Output() logoutRequisitado = new EventEmitter<void>();
}
