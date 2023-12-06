import { Component, Input, OnInit } from '@angular/core';
import { Control } from '../../class/base-form-component/base-form-component'

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss']
})
export class InputComponent implements OnInit {
  @Input({required: true}) public data!: Control;

  constructor() { }

  public blur(): void {
    this.data = this.data.blur(this.data);
  }

  public ngOnInit(): void {
    this.data.errors = this.data.errors.sort((a, b) => a.id - b.id);
  }

  public showError(key: string): boolean {
    if (!this.data.control.errors || !this.data.control.hasError(key))
      return false;

    for (let error of this.data.errors) {
      if (this.data.control.hasError(error.key)) {
        return error.key === key && (error.serverError || this.data.control.dirty);
      }
    }
    return false;
  }
}
