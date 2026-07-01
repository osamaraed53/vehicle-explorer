import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from '@angular/core';

@Component({
  selector: 'app-alert',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './alert.component.html',
})
export class AlertComponent {
  readonly message = input.required<string>();
  readonly retryable = input(false);
  readonly retry = output<void>();
}
