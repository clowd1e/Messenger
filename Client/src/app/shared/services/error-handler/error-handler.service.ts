import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { HttpError } from '../../models/errors/HttpError';
import { HttpValidationError } from '../../models/errors/HttpValidationError';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  toastr = inject(ToastrService);

  handleHttpError(error: HttpErrorResponse): void {
    switch (error.status) {
      case HttpStatusCode.NotFound:
        this.processClientError(error);
        break;
      case HttpStatusCode.BadRequest:
        this.processClientError(error);
        break;
      case HttpStatusCode.InternalServerError:
        this.toastr.error('Server error.');
        break;
      default:
        this.toastr.error('Unknown error.');
        break;
    }
  }

  private processClientError(error: HttpErrorResponse) {
    if (this.isHttpValidationError(error.error)) {
      let apiError: HttpValidationError = error.error;

      for (let error of apiError.errors) {
        this.toastr.error(error.description, error.code)
      }
    } else {
      let apiError: HttpError = error.error;
      
      let description: string = apiError.errors.description;
      this.toastr.error(description, 'Error');
    }
  }

  private isHttpValidationError(error: any): error is HttpValidationError {
    return error && Array.isArray(error.errors);
  }
}
