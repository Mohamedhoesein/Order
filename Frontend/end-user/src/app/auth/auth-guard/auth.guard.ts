import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  return inject(AuthService).loggedIn()
    .pipe(
      map((response) => {
        if (response.loggedIn) {
            return true;
        }
        inject(Router).navigate(['/login']);
        return false;
      }),
      catchError((error) => {
        inject(Router).navigate(['/login']);
          return of(false);
      })
    );
};
