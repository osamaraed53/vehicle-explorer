import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';



export class ApiError extends Error {
  constructor(
    message: string,
    readonly status: number,
  ) {
    super(message);
    this.name = 'ApiError';
  }
}


export const apiErrorInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse) {
        return throwError(() => new ApiError(toMessage(error), error.status));
      }
      return throwError(() => error);
    }),
  );


  function toMessage(error: HttpErrorResponse): string {
  if (error.status === 0) {
    return 'Cannot reach the server. Please check that the API is running and try again.';
  }


  switch (error.status) {
    case 400:
      return 'The request was invalid. Please adjust your selection and try again.';
    case 404:
      return 'No data was found for the selected criteria.';
    case 502:
      return 'The vehicle data provider is currently unavailable. Please try again shortly.';
    default:
      return `Something went wrong (HTTP ${error.status}). Please try again.`;
  }
}
