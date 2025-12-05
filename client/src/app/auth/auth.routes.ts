import { Routes } from '@angular/router';
import { Login } from './login/login';
import { Registro } from './registro/registro';
import { provideNgxMask } from 'ngx-mask';

export const authRoutes: Routes = [
  { path: 'login', component: Login },
  { path: 'registro', component: Registro, providers: [provideNgxMask()] },
];
