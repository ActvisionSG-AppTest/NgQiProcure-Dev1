import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductPricesListingComponent } from './product-prices-listing.component';

describe('ProductPricesListingComponent', () => {
  let component: ProductPricesListingComponent;
  let fixture: ComponentFixture<ProductPricesListingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductPricesListingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductPricesListingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
