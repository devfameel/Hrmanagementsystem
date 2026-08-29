import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Menu } from '../../../core/services/menu';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './layout.html',
  styleUrl: './layout.css'
})
export class Layout implements OnInit {
  private menuService = inject(Menu);
  private router = inject(Router);

  menuItems: any[] = [];

  ngOnInit() {
    this.menuService.getUserMenu().subscribe({
      next: (data) => {
        this.menuItems = data;
      },
      error: (err) => {
        console.error('Failed to load menu', err);
      }
    });
  }

  logout() {
    // Clear local storage and redirect to login
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
