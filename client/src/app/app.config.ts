import {
  ApplicationConfig,
  DEFAULT_CURRENCY_CODE,
  inject,
  LOCALE_ID,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { CanActivateFn, provideRouter, Router, Routes } from '@angular/router';
import { take, map } from 'rxjs';
import { AuthService } from './auth/auth.service';
import { provideAuth } from './auth/auth.provider';
import { provideNotifications } from './shared/notificacao/notificacao.provider';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

registerLocaleData(localePt);

const usuarioDesconhecidoGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.obterAccessToken().pipe(
    take(1),
    map((token) => (!token ? true : router.createUrlTree(['/inicio']))),
  );
};

const usuarioAutenticadoGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.obterAccessToken().pipe(
    take(1),
    map((token) => (token ? true : router.createUrlTree(['/auth/login']))),
  );
};

const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.routes').then((r) => r.authRoutes),
    canMatch: [usuarioDesconhecidoGuard],
  },
  {
    path: 'inicio',
    loadComponent: () => import('./inicio/inicio').then((c) => c.Inicio),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'clientes',
    loadChildren: () => import('./cliente/cliente.routes').then((r) => r.clienteRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'condutores',
    loadChildren: () => import('./condutor/condutor.routes').then((r) => r.condutorRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'grupos-veiculos',
    loadChildren: () =>
      import('./grupo-veiculo/grupoVeiculo.routes').then((r) => r.grupoVeiculoRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'veiculos',
    loadChildren: () => import('./veiculo/veiculo.routes').then((r) => r.veiculoRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'planos-cobrancas',
    loadChildren: () =>
      import('./plano-cobranca/planoCobranca.routes').then((r) => r.planoCobrancaRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'taxas-servicos',
    loadChildren: () =>
      import('./taxa-servico/taxaServico.routes').then((r) => r.taxaServicoRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'alugueis',
    loadChildren: () => import('./aluguel/aluguel.routes').then((r) => r.aluguelRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'preco-combustivel',
    loadChildren: () =>
      import('./preco-combustivel/precoCombustivel.routes').then((r) => r.precoCombustivelRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
  {
    path: 'funcionarios',
    loadChildren: () => import('./funcionario/funcionario.routes').then((r) => r.funcionarioRoutes),
    canActivate: [usuarioAutenticadoGuard],
  },
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),

    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },

    provideNotifications(),
    provideAuth(),
  ],
};
