import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployeeService } from '../../../../core/services/employee.service';
import { DepartmentService } from '../../../../core/services/department.service';
import { Employee } from '../../../../core/models/employee.model';
import { Department } from '../../../../core/models/department.model';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './employee.html',
  styleUrl: './employee.css',
})
export class EmployeeComponent implements OnInit {
  private employeeService = inject(EmployeeService);
  private departmentService = inject(DepartmentService);
  private fb = inject(FormBuilder);
  private cdr = inject(ChangeDetectorRef);

  employees: Employee[] = [];
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
  currentEmployeeId: number | null = null;
  employeeForm!: FormGroup;

  // React Design Constants
  avatarPalette = ["#3E76A8", "#4F8AC2", "#2E6690", "#5C97C9", "#1F5E88", "#4680B0"];
  deptColors: Record<string, { bg: string, fg: string }> = {
    Engineering: { bg: "#DCEEFB", fg: "#1E5F8F" },
    Design: { bg: "#E4F1FB", fg: "#2A6FA0" },
    HR: { bg: "#D6EAFA", fg: "#1B5480" },
    Finance: { bg: "#E9F4FC", fg: "#356F9C" },
    Sales: { bg: "#DFEFFB", fg: "#25638E" },
    Marketing: { bg: "#D2E8F9", fg: "#175077" },
  };

  ngOnInit() {
    this.initForm();
    this.loadDepartments();
    this.loadEmployees();
  }

  // Combobox State
  showDeptDropdown = false;
  deptSearchText = '';
  filteredDepartments: Department[] = [];
  selectedDepartmentName = '';

  // Department Modal State
  isDeptModalOpen = false;
  departmentForm!: FormGroup;

  initForm() {
    this.employeeForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      departmentId: [null, Validators.required]
    });

    this.departmentForm = this.fb.group({
      name: ['', Validators.required],
      departemntCode: ['']
    });
  }

  loadDepartments() {
    this.departmentService.getDepartments(1, 100).subscribe({
      next: (res) => {
        this.departments = res.items || [];
        this.filteredDepartments = [...this.departments];
      },
      error: (err) => console.error('Failed to load departments', err)
    });
  }

  // --- Combobox Logic ---
  onDeptSearch(event: any) {
    this.deptSearchText = event.target.value;
    const q = this.deptSearchText.toLowerCase();
    this.filteredDepartments = this.departments.filter(d => d.name.toLowerCase().includes(q));
    
    // If exact match doesn't exist, we clear the form control (until they click "Add" or a valid option)
    const exactMatch = this.departments.find(d => d.name.toLowerCase() === q);
    if (exactMatch) {
      this.employeeForm.patchValue({ departmentId: exactMatch.id });
      this.selectedDepartmentName = exactMatch.name;
    } else {
      this.employeeForm.patchValue({ departmentId: null });
      this.selectedDepartmentName = '';
    }
  }

  selectDepartment(dept: Department) {
    this.deptSearchText = dept.name;
    this.selectedDepartmentName = dept.name;
    this.employeeForm.patchValue({ departmentId: dept.id });
    this.showDeptDropdown = false;
  }

  hideDeptDropdown() {
    setTimeout(() => {
      this.showDeptDropdown = false;
      // Revert if they didn't select a valid department
      if (!this.employeeForm.value.departmentId) {
        this.deptSearchText = '';
      } else {
        this.deptSearchText = this.selectedDepartmentName;
      }
    }, 200);
  }

  quickAddDepartment() {
    this.showDeptDropdown = false;
    this.departmentForm.reset();
    this.departmentForm.patchValue({ name: this.deptSearchText.trim() });
    this.isDeptModalOpen = true;
  }

  closeDeptModal() {
    this.isDeptModalOpen = false;
  }

  onDeptSubmit() {
    if (this.departmentForm.invalid) return;

    this.departmentService.createDepartment(this.departmentForm.value).subscribe({
      next: (res) => {
        // Add to list and select it
        this.departments.push(res);
        this.selectDepartment(res);
        this.closeDeptModal();
      },
      error: (err) => console.error('Failed to create department', err)
    });
  }
  // ------------------------

  loadEmployees() {
    this.isLoading = true;
    
    // If search is implemented in the backend, you'd call a search endpoint here.
    // Assuming the backend has the /search endpoint we created earlier:
    if (this.searchQuery.trim()) {
      // NOTE: employee.service needs search method if you want true backend search.
      // But based on our earlier C# code, /search exists but isn't paginated.
      // For simplicity, let's just use getEmployees if no search, or filter on frontend if you want.
      // Assuming you added a search method or just using getEmployees for now.
      this.employeeService.getEmployees(1, 100).subscribe({
        next: (res) => {
          const q = this.searchQuery.toLowerCase();
          this.employees = (res.items || []).filter(e => 
            e.firstName.toLowerCase().includes(q) || 
            e.lastName.toLowerCase().includes(q) || 
            e.email.toLowerCase().includes(q)
          );
          this.totalCount = this.employees.length;
          this.totalPages = 1;
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.employeeService.getEmployees(this.currentPage, this.pageSize).subscribe({
        next: (res) => {
          this.employees = res.items || [];
          this.currentPage = res.currentPage;
          this.totalPages = res.totalPage;
          this.totalCount = res.totalCount;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Failed to load employees', err);
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  onSearchChange(event: any) {
    this.searchQuery = event.target.value;
    this.loadEmployees();
  }

  // Helpers for Design
  getInitials(first: string, last: string) {
    if (!first) return 'NA';
    const f = first[0] ? first[0].toUpperCase() : '';
    const l = last && last[0] ? last[0].toUpperCase() : '';
    return `${f}${l}`;
  }

  getAvatarColor(name: string) {
    let hash = 0;
    for (let i = 0; i < name.length; i++) hash = name.charCodeAt(i) + ((hash << 5) - hash);
    return this.avatarPalette[Math.abs(hash) % this.avatarPalette.length];
  }

  getDeptColors(deptName: string | undefined) {
    if (!deptName) return { bg: "#F1EEE4", fg: "#7A7460" };
    return this.deptColors[deptName] ?? { bg: "#F1EEE4", fg: "#7A7460" };
  }

  // Modal
  openAddModal() {
    this.isEditMode = false;
    this.currentEmployeeId = null;
    this.employeeForm.reset();
    this.deptSearchText = '';
    this.selectedDepartmentName = '';
    this.isModalOpen = true;
  }

  openEditModal(emp: Employee) {
    this.isEditMode = true;
    this.currentEmployeeId = emp.id;
    this.employeeForm.patchValue(emp);
    this.deptSearchText = emp.department?.name || '';
    this.selectedDepartmentName = this.deptSearchText;
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  onSubmit() {
    if (this.employeeForm.invalid) return;

    if (this.isEditMode && this.currentEmployeeId) {
      this.employeeService.updateEmployee(this.currentEmployeeId, this.employeeForm.value).subscribe({
        next: () => {
          this.closeModal();
          this.loadEmployees();
        }
      });
    } else {
      this.employeeService.createEmployee(this.employeeForm.value).subscribe({
        next: () => {
          this.closeModal();
          this.loadEmployees();
        }
      });
    }
  }

  deleteEmployee(id: number) {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => this.loadEmployees()
      });
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadEmployees();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadEmployees();
    }
  }
}
