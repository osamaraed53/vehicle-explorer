import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  output,
} from '@angular/core';

const GAP = '…' as const;
type PageToken = number | typeof GAP;

@Component({
  selector: 'app-pagination',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './pagination.component.html',
})
export class PaginationComponent {
  readonly page = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalItems = input.required<number>();
  readonly pageChange = output<number>();
  protected readonly gap = GAP;
  
  protected readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalItems() / this.pageSize())),
  );

  protected readonly rangeStart = computed(() =>
    this.totalItems() === 0 ? 0 : (this.page() - 1) * this.pageSize() + 1,
  );
  protected readonly rangeEnd = computed(() =>
    Math.min(this.page() * this.pageSize(), this.totalItems()),
  );

  protected readonly hasPrev = computed(() => this.page() > 1);
  protected readonly hasNext = computed(() => this.page() < this.totalPages());

  protected readonly pages = computed<readonly PageToken[]>(() => {
    const total = this.totalPages();
    const current = this.page();
    if (total <= 7) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    const tokens: PageToken[] = [1];
    const start = Math.max(2, current - 1);
    const end = Math.min(total - 1, current + 1);

    if (start > 2) {
      tokens.push(GAP);
    }
    for (let p = start; p <= end; p++) {
      tokens.push(p);
    }
    if (end < total - 1) {
      tokens.push(GAP);
    }
    tokens.push(total);
    return tokens;
  });

  protected go(page: number): void {
    if (page >= 1 && page <= this.totalPages() && page !== this.page()) {
      this.pageChange.emit(page);
    }
  }
}
