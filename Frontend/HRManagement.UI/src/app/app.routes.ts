import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Layout } from './features/dashboard/layout/layout';
import { Employee } from './features/dashboard/pages/employee/employee';
import { Leave } from './features/dashboard/pages/leave/leave';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { 
    path: 'dashboard', 
    component: Layout,
    children: [
      { path: '', redirectTo: 'employee', pathMatch: 'full' },
      { path: 'employee', component: Employee },
      { path: 'leave', component: Leave }
    ]
  }
];
