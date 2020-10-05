import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ParamSettingsListingComponent } from './param-settings-listing.component';

describe('ParamSettingsListingComponent', () => {
  let component: ParamSettingsListingComponent;
  let fixture: ComponentFixture<ParamSettingsListingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParamSettingsListingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParamSettingsListingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
