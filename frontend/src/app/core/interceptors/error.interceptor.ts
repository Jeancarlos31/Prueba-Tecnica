import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AlertService } from '../services/alert.service';

/**
 * Interceptor funcional que revisa toda respuesta HTTP proveniente del
 * backend. Si el status no es exitoso (fuera del rango 2xx), muestra una
 * alerta al usuario con un mensaje descriptivo según el código HTTP.
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const alertService = inject(AlertService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      alertService.show(buildMessage(error), 'danger');
      return throwError(() => error);
    })
  );
};

function buildMessage(error: HttpErrorResponse): string {
  if (error.status === 0) {
    return 'No fue posible conectar con el servidor. Verifique que el backend esté en ejecución.';
  }
  if (error.status === 404) {
    return extractServerMessage(error) ?? `No se encontró el recurso solicitado (404).`;
  }
  if (error.status === 400) {
    return extractServerMessage(error) ?? 'Los datos enviados no son válidos (400).';
  }
  if (error.status >= 500) {
    return 'Error interno del servidor. Intente nuevamente más tarde.';
  }
  return `Ocurrió un error al comunicarse con el servidor (código ${error.status}).`;
}

function extractServerMessage(error: HttpErrorResponse): string | null {
  const body = error.error;
  if (!body) return null;
  if (typeof body === 'string') return body;
  if (typeof body.message === 'string') return body.message;

  // ValidationProblemDetails de ASP.NET Core: { title, errors: { campo: [mensajes] } }
  if (body.title) {
    const detalles = body.errors
      ? Object.values(body.errors as Record<string, string[]>).flat().join(' ')
      : '';
    return `${body.title} ${detalles}`.trim();
  }
  return null;
}
