import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleExplorerComponent } from './vehicle-explorer.component';

describe('VehicleExplorerComponent', () => {
  let component: VehicleExplorerComponent;
  let fixture: ComponentFixture<VehicleExplorerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VehicleExplorerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VehicleExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
