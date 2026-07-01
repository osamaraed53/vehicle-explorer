
import {  Injectable, computed, inject, signal } from '@angular/core';
import { ApiError } from '../../core/interceptors/api-error.interceptor';
import { Make, VehicleModel, VehicleType } from '../../core/models';
import { VehicleApiService } from '../../core/services/vehicle-api.service';
import { PAGE_SIZE } from './vehicle-explorer.constants';

interface AsyncSlice<T> {
  readonly value: T;
  readonly loading: boolean;
  readonly error: string | null;
}
function idle<T>(value: T): AsyncSlice<T> {
  return { value, loading: false, error: null };
}

@Injectable()
export class VehicleExplorerStore {
  private readonly api = inject(VehicleApiService);

  private readonly makesSlice = signal<AsyncSlice<readonly Make[]>>(idle([]));
  private readonly vehicleTypesSlice = signal<AsyncSlice<readonly VehicleType[]>>(idle([]));
  private readonly modelsSlice = signal<AsyncSlice<readonly VehicleModel[]>>(idle([]));
 
 
  private readonly _makesCount = signal(0);

  readonly makes = computed(() => this.makesSlice().value);
  readonly makesLoading = computed(() => this.makesSlice().loading);
  readonly makesError = computed(() => this.makesSlice().error);
  readonly makesCount = this._makesCount.asReadonly();



  searchMakes(query: string): void {
    this.fetchMakes(query.trim() || null);
  }

  private fetchMakes(search: string | null): void {
    this.makesSlice.set({ value: this.makesSlice().value, loading: true, error: null });
    this.api
      .getMakes(1, PAGE_SIZE, search)
      .subscribe({
        next: (result) => {
          this.makesSlice.set({ value: result.data, loading: false, error: null });
          this._makesCount.set(result.count);
        },
        error: (err) => this.makesSlice.set({ value: [], loading: false, error: messageOf(err) }),
      });
  }

}

function messageOf(error: unknown): string {
  if (error instanceof ApiError || error instanceof Error) {
    return error.message;
  }
    return 'An unexpected error occurred. Please try again.';
}
