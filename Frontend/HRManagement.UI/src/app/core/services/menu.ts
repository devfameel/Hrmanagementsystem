import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Menu {
  // Dummy data for now, replace with actual HTTP call later
  getUserMenu(): Observable<any[]> {
    return of([
      { id: 1, name: 'Employee', route: '/dashboard/employee', icon: '👥' },
      { id: 2, name: 'Leave', route: '/dashboard/leave', icon: '📅' }
    ]);
  }
}
