import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="flex flex-col items-center justify-center gap-2 px-6 py-12 text-center text-slate-400"
    >
      <svg
        class="h-10 w-10"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        stroke-width="1.5"
        aria-hidden="true"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M3.75 9.75h16.5m-16.5 4.5h16.5m-13.5-9v13.5m-3-13.5h19.5a.75.75 0 01.75.75v12a.75.75 0 01-.75.75H2.25a.75.75 0 01-.75-.75v-12a.75.75 0 01.75-.75z"
        />
      </svg>
      <p class="text-sm font-medium text-slate-500">{{ title() }}</p>
      @if (hint()) {
        <p class="max-w-xs text-xs text-slate-400">{{ hint() }}</p>
      }
    </div>
  `,
})
export class EmptyStateComponent {
  readonly title = input.required<string>();
  readonly hint = input<string>();
}
