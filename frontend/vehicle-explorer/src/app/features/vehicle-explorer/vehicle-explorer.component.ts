import { ChangeDetectionStrategy, Component, OnInit, inject } from '@angular/core';

import { VehicleExplorerStore } from './vehicle-explorer.store';
import { VehicleFiltersComponent } from './components/vehicle-filters/vehicle-filters.component';
import { ModelsTableComponent } from './components/models-table/models-table.component';

@Component({
  selector: 'app-vehicle-explorer',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [VehicleExplorerStore],
  imports: [VehicleFiltersComponent, ModelsTableComponent],
  templateUrl: './vehicle-explorer.component.html',
})
export class VehicleExplorerComponent implements OnInit {
  private readonly store = inject(VehicleExplorerStore);

  ngOnInit(): void {
    this.store.loadMakes();
  }
}
