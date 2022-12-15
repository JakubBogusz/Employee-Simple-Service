import { Component, OnInit } from '@angular/core';
import { Employee } from 'src/app/models/employee.model';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent implements OnInit {

  addEmployeeRequest: Employee = {
    id: '',
    firstName: '',
    lastName: '',
    streetName: '',
    houseNumber: '',
    apartmentNumber: null,
    postalCode: '',
    town: '',
    phoneNumber: '',
    dateOfBirth: Date.prototype,
    age: 0
  }
  constructor() {
    
  }
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  addEmployee() {
    console.log(this.addEmployeeRequest);
  }
}
