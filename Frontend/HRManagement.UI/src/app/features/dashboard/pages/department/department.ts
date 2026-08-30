import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DepartmentService } from '../../../../core/services/department.service';
import { Department } from '../../../../core/models/department.model';

@Component({
  selector: 'app-department',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './department.html',
  styleUrl: './department.css',
})
export class DepartmentComponent implements OnInit {
  private departmentService = inject(DepartmentService);
  private fb = inject(FormBuilder);
  private cdr = inject(ChangeDetectorRef);

  departments: Department[] = [];
  
  // Pagination & Search
  currentPage = 1;
  pageSize = 7;
  totalPages = 1;
  searchQuery = '';
  totalCount = 0;

  isLoading = true;

  // Modal State
  isModalOpen = false;
  isEditMode = false;
  currentDepartmentId: number | null = null;
  departmentForm!: FormGroup;

  ngOnInit() {
    this.initForm();
    this.loadDepartments();
  }

  initForm() {
    this.departmentForm = this.fb.group({
      name: ['', Validators.required],
      departemntCode: ['']
    });
  }

  loadDepartments() {
    this.isLoading = true;
    
    if (this.searchQuery.trim()) {
      this.departmentService.getDepartments(1, 1000).subscribe({
        next: (res) => {
          const q = this.searchQuery.toLowerCase();
          this.departments = (res.items || []).filter(d => 
            d.name.toLowerCase().includes(q) || 
            (d.departemntCode && d.departemntCode.toLowerCase().includes(q))
          );
          this.totalCount = this.departments.length;
          this.totalPages = 1;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Failed to load departments', err);
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.departmentService.getDepartments(this.currentPage, this.pageSize).subscribe({
        next: (res) => {
          this.departments = res.items || [];
          this.currentPage = res.currentPage;
          this.totalPages = res.totalPage;
          this.totalCount = res.totalCount;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Failed to load departments', err);
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  onSearchChange(event: any) {
    this.searchQuery = event.target.value;
    this.loadDepartments();
  }

  // Modal
  openAddModal() {
    this.isEditMode = false;
    this.currentDepartmentId = null;
    this.departmentForm.reset();
    this.isModalOpen = true;
  }

  openEditModal(dept: Department) {
    this.isEditMode = true;
    this.currentDepartmentId = dept.id;
    this.departmentForm.patchValue(dept);
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  onSubmit() {
    if (this.departmentForm.invalid) return;

    if (this.isEditMode && this.currentDepartmentId) {
      this.departmentService.updateDepartment(this.currentDepartmentId, this.departmentForm.value).subscribe({
        next: () => {
          this.closeModal();
          this.loadDepartments();
        }
      });
    } else {
      this.departmentService.createDepartment(this.departmentForm.value).subscribe({
        next: () => {
          this.closeModal();
          this.loadDepartments();
        }
      });
    }
  }

  deleteDepartment(id: number) {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentService.deleteDepartment(id).subscribe({
        next: () => this.loadDepartments()
      });
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadDepartments();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadDepartments();
    }
  }
}
