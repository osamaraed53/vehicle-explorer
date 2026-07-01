
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';

import { Make, VehicleType } from '../../../../core/models';
import { SearchableSelectComponent } from '../searchable-select/searchable-select.component';
import { VehicleExplorerStore } from '../../vehicle-explorer.store';

@Component({
  selector: 'app-vehicle-filters',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [SearchableSelectComponent],
  templateUrl: './vehicle-filters.component.html',
})
export class VehicleFiltersComponent {
  protected readonly store = inject(VehicleExplorerStore);

  protected readonly makeLabel = (make: Make): string => make.name;

  protected onMakeSelected(make: Make | null): void {
    this.store.selectMake(make);
  }

  protected onMakeSearch(query: string): void {
    this.store.searchMakes(query);
  }

  protected onYearChange(value: string): void {
    this.store.setYear(value ? Number(value) : null);
  }

  protected onVehicleTypeChange(value: string): void {
    const type: VehicleType | null = value
      ? this.store.vehicleTypes().find((t) => t.id === Number(value)) ?? null
      : null;
    this.store.setVehicleType(type);
  }

  protected onSearch(): void {
    this.store.searchModels();
  }
}
