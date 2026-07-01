import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { EmptyStateComponent } from '../../../../components/empty-state/empty-state.component';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { AlertComponent } from '../../../../components/alert/alert.component';
import { PaginationComponent } from '../../../../components/pagination/pagination.component';
import { VehicleExplorerStore } from '../../vehicle-explorer.store';

@Component({
  selector: 'app-models-table',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    EmptyStateComponent,
    SpinnerComponent,
    AlertComponent,
    PaginationComponent,
  ],
  templateUrl: './models-table.component.html',
})
export class ModelsTableComponent {
  protected readonly store = inject(VehicleExplorerStore);

  protected onSearch(): void {
    this.store.searchModels();
  }
}
