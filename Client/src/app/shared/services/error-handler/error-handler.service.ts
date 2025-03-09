import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { HttpError } from '../../models/errors/HttpError';
import { HttpValidationError } from '../../models/errors/HttpValidationError';
import { Error } from '../../models/errors/Error';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  toastr = inject(ToastrService);

  handleHttpError(error: HttpErrorResponse): void {
    switch (error.status) {
      case HttpStatusCode.NotFound:
        this.processHttpError(error);
        break;
      case HttpStatusCode.BadRequest:
        this.processHttpError(error);
        break;
      case HttpStatusCode.InternalServerError:
        this.toastr.error('Server error.');
        break;
      default:
        this.toastr.error('Unknown error.');
        break;
    }
  }

  handleError(error: any): void {
    this.processClientError(error);
  }

  private processHttpError(error: HttpErrorResponse): void {
    this.processClientError(error.error);
  }

  private processClientError(error: any) {
    if (this.isHttpValidationError(error)) {
      let apiError: HttpValidationError = error;

      for (let error of apiError.errors) {
        this.toastr.error(error.description, error.code)
      }
    } else {
      let apiError: HttpError = error;
      
      let description: string = apiError.errors.description;
      this.toastr.error(description, 'Error');
    }
  }

  private isHttpValidationError(error: any): error is HttpValidationError {
    return error && Array.isArray(error.errors);
  }
}
