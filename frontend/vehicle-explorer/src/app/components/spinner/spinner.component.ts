import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'app-spinner',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="flex items-center justify-center gap-3 text-slate-500"
      role="status"
      aria-live="polite"
    >
      <svg
        class="h-5 w-5 animate-spin text-brand-600"
        viewBox="0 0 24 24"
        fill="none"
        aria-hidden="true"
      >
        <circle
          class="opacity-25"
          cx="12"
          cy="12"
          r="10"
          stroke="currentColor"
          stroke-width="4"
        ></circle>
        <path
          class="opacity-75"
          fill="currentColor"
          d="M4 12a8 8 0 018-8V0C5.4 0 0 5.4 0 12h4z"
        ></path>
      </svg>
      @if (label()) {
        <span class="text-sm font-medium">{{ label() }}</span>
      }
    </div>
  `,
})
export class SpinnerComponent {
  readonly label = input<string>();
}
