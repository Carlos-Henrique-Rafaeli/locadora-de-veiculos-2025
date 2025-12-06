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
