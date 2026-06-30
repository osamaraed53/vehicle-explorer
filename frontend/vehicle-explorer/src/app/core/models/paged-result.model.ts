export interface PagedResult<T> {
  readonly data: readonly T[];
  readonly count: number;
}
