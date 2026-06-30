import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Make, PagedResult, VehicleModel, VehicleType } from '../models';
import {} from '../models/paged-result.model';



@Injectable({
  providedIn: 'root',
})
export class VehicleApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;
  private static readonly MAX_PAGE_SIZE = 10;

  getMakes(page: number, pageSize: number): Observable<PagedResult<Make>> {
    return this.http.get<PagedResult<Make>>(`${this.baseUrl}/makes`, {
      params: this.pageParams(page, pageSize),
    });
  }

  getVehicleTypes(
    makeId: number,
    page: number,
    pageSize: number,
  ): Observable<PagedResult<VehicleType>> {
    return this.http.get<PagedResult<VehicleType>>(
      `${this.baseUrl}/makes/${makeId}/vehicle-types`,
      {
        params: this.pageParams(page, pageSize),
      },
    );
  }

  getModels(
    makeId: number,
    year: number,
    page: number,
    pageSize: number,
    vehicleType?: string,
  ): Observable<PagedResult<VehicleModel>> {
    let params = this.pageParams(page, pageSize).set('Year', year);
    if (vehicleType) {
      params = params.set('VehicleType', vehicleType);
    }
    return this.http.get<PagedResult<VehicleModel>>(
      `${this.baseUrl}/makes/${makeId}/models`,
      {
        params,
      },
    );
  }

  private pageParams(page: number, pageSize: number): HttpParams {
    return new HttpParams().set('Page', page).set('PageSize', pageSize);
  }


}
