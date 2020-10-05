import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServicePricesListingComponent } from './service-prices-listing.component';

describe('ServicePricesListingComponent', () => {
  let component: ServicePricesListingComponent;
  let fixture: ComponentFixture<ServicePricesListingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServicePricesListingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServicePricesListingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
