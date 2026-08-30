export interface Department {
  id: number;
  name: string;
  departemntCode?: string;
  isDeleted?: boolean;
  createdDate?: string;
  createdUserId?: number;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  currentPage: number;
  totalPage: number;
}
