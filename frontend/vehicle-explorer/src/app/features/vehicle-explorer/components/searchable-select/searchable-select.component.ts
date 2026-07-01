import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  computed,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-searchable-select',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './searchable-select.component.html',
})
export class SearchableSelectComponent<T> {
  private readonly host = inject<ElementRef<HTMLElement>>(ElementRef);

  readonly items = input.required<readonly T[]>();
  readonly allItemsCount = input.required<number>();
  readonly displayWith = input.required<(item: T) => string>();
  readonly selected = input<T | null>(null);
  readonly label = input.required<string>();
  readonly inputId = input.required<string>();
  readonly placeholder = input('Search…');
  readonly disabled = input(false);
  readonly loading = input(false);

  readonly selectionChange = output<T | null>();
  readonly searchChange = output<string>();

  protected readonly open = signal(false);
  protected readonly query = signal('');
  protected readonly activeIndex = signal(0);

  protected readonly hiddenCount = computed(() =>
    Math.max(0, this.allItemsCount() - this.items().length),
  );

  private readonly searchTerms = new Subject<string>();

  constructor() {
    this.searchTerms
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed())
      .subscribe((term) => this.searchChange.emit(term));
  }

  protected display(item: T): string {
    return this.displayWith()(item);
  }

  protected onFocus(): void {
    if (this.disabled()) {
      return;
    }
    this.activeIndex.set(0);
    this.open.set(true);
  }

  protected onInput(value: string): void {
    this.query.set(value);
    this.activeIndex.set(0);
    this.open.set(true);
    this.searchTerms.next(value.trim());
  }

  protected choose(item: T): void {
    this.selectionChange.emit(item);
    this.query.set(this.display(item));
    this.open.set(false);
  }

  protected clear(): void {
    this.selectionChange.emit(null);
    this.query.set('');
    this.open.set(false);
    this.searchTerms.next('');
  }

  protected onKeydown(event: KeyboardEvent): void {
    if (this.disabled()) {
      return;
    }

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.open.set(true);
        this.moveActive(1);
        break;
      case 'ArrowUp':
        event.preventDefault();
        this.moveActive(-1);
        break;
      case 'Enter': {
        const item = this.items()[this.activeIndex()];
        if (this.open() && item !== undefined) {
          event.preventDefault();
          this.choose(item);
        }
        break;
      }
      case 'Escape':
        this.closeAndRestore();
        break;
    }
  }

  private moveActive(delta: number): void {
    const count = this.items().length;
    if (count === 0) {
      return;
    }
    const next = (this.activeIndex() + delta + count) % count;
    this.activeIndex.set(next);
  }

  private closeAndRestore(): void {
    const current = this.selected();
    this.query.set(current ? this.display(current) : '');
    this.open.set(false);
  }

  @HostListener('document:click', ['$event'])
  protected onDocumentClick(event: MouseEvent): void {
    if (
      this.open() &&
      !this.host.nativeElement.contains(event.target as Node)
    ) {
      this.closeAndRestore();
    }
  }
}
