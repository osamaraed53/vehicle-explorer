import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    title: 'Vehicle Explorer',
    loadComponent: () =>
      import('./features/vehicle-explorer/vehicle-explorer.component').then(
        (m) => m.VehicleExplorerComponent,
      ),
  },
  {
    path: '**',
    title: 'Page not found',
    loadComponent: () =>
      import('./features/not-found/not-found.component').then(
        (m) => m.NotFoundComponent,
      ),
  },
];
