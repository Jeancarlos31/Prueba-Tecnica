import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type AlertType = 'success' | 'danger' | 'warning' | 'info';

export interface AlertMessage {
  type: AlertType;
  text: string;
}

/**
 * Servicio central para mostrar alertas al usuario (bootstrap alert).
 * Lo usa el interceptor HTTP para notificar respuestas que no sean
 * exitosas, y también los componentes para confirmar operaciones OK.
 */
@Injectable({ providedIn: 'root' })
export class AlertService {
  private readonly messageSubject = new BehaviorSubject<AlertMessage | null>(null);
  readonly message$ = this.messageSubject.asObservable();

  private timeoutId: ReturnType<typeof setTimeout> | null = null;

  show(text: string, type: AlertType = 'danger'): void {
    this.messageSubject.next({ type, text });

    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }
    this.timeoutId = setTimeout(() => this.clear(), 6000);
  }

  clear(): void {
    this.messageSubject.next(null);
  }
}
