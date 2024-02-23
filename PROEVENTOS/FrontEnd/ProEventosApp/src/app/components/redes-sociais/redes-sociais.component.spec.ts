/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RedesSociaisComponent } from './redes-sociais.component';

describe('RedesSociaisComponent', () => {
  let component: RedesSociaisComponent;
  let fixture: ComponentFixture<RedesSociaisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedesSociaisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedesSociaisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
