import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Employee } from 'src/app/models/employee.model';
import { EmployeesService } from 'src/app/services/employees.service';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css']
})
export class EmployeesListComponent implements OnInit {

  employees: Employee[] = [];
   
  constructor(private employeesService: EmployeesService,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.employeesService.getAllEmployees()
    .subscribe({
      next: employeesList => {
        this.employees = employeesList
        console.log(employeesList);
      },
      error: (response) => {
          console.log(this.employees);
      }
    })
  }

  isEmployeesRoute() {
    return this.activatedRoute.snapshot.routeConfig?.path?.includes('employees');
  }
}