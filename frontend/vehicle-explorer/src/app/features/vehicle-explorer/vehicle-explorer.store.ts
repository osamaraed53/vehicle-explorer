import { Injectable, computed, inject, signal } from '@angular/core';
import { ApiError } from '../../core/interceptors/api-error.interceptor';
import { Make, VehicleModel, VehicleType } from '../../core/models';
import { VehicleApiService } from '../../core/services/vehicle-api.service';
import { MIN_YEAR, PAGE_SIZE } from './vehicle-explorer.constants';

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
  private readonly vehicleTypesSlice = signal<
    AsyncSlice<readonly VehicleType[]>
  >(idle([]));
  private readonly modelsSlice = signal<AsyncSlice<readonly VehicleModel[]>>(
    idle([]),
  );

  private readonly _modelsCount = signal(0);
  private readonly _modelsPage = signal(1);

  private readonly _makesCount = signal(0);

  readonly makes = computed(() => this.makesSlice().value);
  readonly makesLoading = computed(() => this.makesSlice().loading);
  readonly makesError = computed(() => this.makesSlice().error);
  readonly makesCount = this._makesCount.asReadonly();

  private readonly _selectedMake = signal<Make | null>(null);
  private readonly _selectedYear = signal<number | null>(null);
  private readonly _selectedVehicleType = signal<VehicleType | null>(null);

  private readonly _searched = signal(false);

  readonly vehicleTypes = computed(() => this.vehicleTypesSlice().value);
  readonly vehicleTypesLoading = computed(
    () => this.vehicleTypesSlice().loading,
  );
  readonly vehicleTypesError = computed(() => this.vehicleTypesSlice().error);

  readonly models = computed(() => this.modelsSlice().value);
  readonly modelsLoading = computed(() => this.modelsSlice().loading);
  readonly modelsError = computed(() => this.modelsSlice().error);

  readonly modelsCount = this._modelsCount.asReadonly();
  readonly modelsPage = this._modelsPage.asReadonly();
  readonly modelsPageSize = PAGE_SIZE;

  readonly selectedMake = this._selectedMake.asReadonly();
  readonly selectedYear = this._selectedYear.asReadonly();
  readonly selectedVehicleType = this._selectedVehicleType.asReadonly();
  readonly searched = this._searched.asReadonly();

  searchMakes(query: string): void {
    this.fetchMakes(query.trim() || null);
  }

  private fetchMakes(search: string | null): void {
    this.makesSlice.set({
      value: this.makesSlice().value,
      loading: true,
      error: null,
    });
    this.api.getMakes(1, PAGE_SIZE, search).subscribe({
      next: (result) => {
        this.makesSlice.set({
          value: result.data,
          loading: false,
          error: null,
        });
        this._makesCount.set(result.count);
      },
      error: (err) =>
        this.makesSlice.set({
          value: [],
          loading: false,
          error: messageOf(err),
        }),
    });
  }
  readonly availableYears: readonly number[] = (() => {
    const maxYear = new Date().getFullYear() + 1;
    return Array.from(
      { length: maxYear - MIN_YEAR + 1 },
      (_, i) => maxYear - i,
    );
  })();

  readonly canSearch = computed(
    () => this._selectedMake() !== null && this._selectedYear() !== null,
  );

  loadMakes(): void {
    if (this.makesSlice().value.length > 0 || this.makesSlice().loading) {
      return;
    }
    this.fetchMakes(null);
  }

  selectMake(make: Make | null): void {
    this._selectedMake.set(make);
    this._selectedVehicleType.set(null);
    this.resetModels();

    if (!make) {
      this.vehicleTypesSlice.set(idle([]));
      return;
    }

    this.vehicleTypesSlice.set({ value: [], loading: true, error: null });
    this.api.getVehicleTypes(make.id, 1, PAGE_SIZE).subscribe({
      next: (result) =>
        this.vehicleTypesSlice.set({
          value: result.data,
          loading: false,
          error: null,
        }),
      error: (err) =>
        this.vehicleTypesSlice.set({
          value: [],
          loading: false,
          error: messageOf(err),
        }),
    });
  }

  setYear(year: number | null): void {
    this._selectedYear.set(year);
    this.resetModels();
  }

  // Same for the optional vehicle-type filter.
  setVehicleType(type: VehicleType | null): void {
    this._selectedVehicleType.set(type);
    this.resetModels();
  }

  searchModels(): void {
    if (!this.canSearch()) {
      return;
    }
    this._searched.set(true);
    this.loadModelsPage(1);
  }

  goToModelsPage(page: number): void {
    if (!this._searched() || page === this._modelsPage()) {
      return;
    }
    this.loadModelsPage(page);
  }

  private loadModelsPage(page: number): void {
    const make = this._selectedMake();
    const year = this._selectedYear();
    if (!make || year === null) {
      return;
    }

    this._modelsPage.set(page);
    this.modelsSlice.set({ value: [], loading: true, error: null });
    this.api
      .getModels(
        make.id,
        year,
        page,
        PAGE_SIZE,
        this._selectedVehicleType()?.name,
      )
      .subscribe({
        next: (result) => {
          this.modelsSlice.set({
            value: result.data,
            loading: false,
            error: null,
          });
          this._modelsCount.set(result.count);
        },
        error: (err) =>
          this.modelsSlice.set({
            value: [],
            loading: false,
            error: messageOf(err),
          }),
      });
  }

  private resetModels(): void {
    this.modelsSlice.set(idle([]));
    this._modelsCount.set(0);
    this._modelsPage.set(1);
    this._searched.set(false);
  }
}

function messageOf(error: unknown): string {
  if (error instanceof ApiError || error instanceof Error) {
    return error.message;
  }
  return 'An unexpected error occurred. Please try again.';
}
