import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Layout } from './features/dashboard/layout/layout';
import { EmployeeComponent } from './features/dashboard/pages/employee/employee';
import { DepartmentComponent } from './features/dashboard/pages/department/department';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { 
    path: 'dashboard', 
    component: Layout,
    children: [
      { path: '', redirectTo: 'employee', pathMatch: 'full' },
      { path: 'employee', component: EmployeeComponent },
      { path: 'department', component: DepartmentComponent }
    ]
  }
];
