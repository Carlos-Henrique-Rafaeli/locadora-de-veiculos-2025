import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { ShellComponent } from './shared/shell/shell.component';
import { PartialObserver } from 'rxjs';
import { AuthService } from './auth/auth.service';
import { NotificacaoService } from './shared/notificacao/notificacao.service';

@Component({
  selector: 'app-root',
  imports: [ShellComponent, RouterOutlet, AsyncPipe],
  template: `
    @if (accessToken$ | async; as accessToken) {
      <app-shell
        [usuarioAutenticado]="accessToken.usuarioAutenticado"
        (logoutRequisitado)="logout()"
      >
        <router-outlet></router-outlet>
      </app-shell>
    } @else {
      <main class="container-fluid py-3">
        <router-outlet></router-outlet>
      </main>
    }
  `,
})
export class App {
  protected readonly router = inject(Router);
  protected readonly authService = inject(AuthService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly accessToken$ = this.authService.obterAccessToken();

  protected logout() {
    const sairObserver: PartialObserver<null> = {
      error: (err) => this.notificacaoService.erro(err.message),
      complete: () => this.router.navigate(['/auth', 'login']),
    };

    this.authService.sair().subscribe(sairObserver);
  }
}
