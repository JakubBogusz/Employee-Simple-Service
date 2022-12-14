import { Component, OnInit } from '@angular/core';
import { Employee } from 'src/app/models/emploee';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css']
})
export class EmployeesListComponent implements OnInit {

  employees: Employee[] = [
    {
      Id: '0a906621-1e06-480c-9afb-7f92c1acbb59',
      FirstName: 'Michael',
      LastName: 'Jordan',
      StreetName: 'Mair Avenue',
      HouseNumber: '76',
      ApartmentNumber: 12,
      PostalCode: '30-202',
      Town: 'Warsaw',
      PhoneNumber: 888234994,
      DateOfBirth: new Date("1980-01-16"),
      Age: 42
    },
    {
      Id: '9f2c3263-15f4-4e9e-9fd3-465be7cf4d7e',
      FirstName: 'Lebron',
      LastName: 'James',
      StreetName: 'CA Street',
      HouseNumber: '156C',
      ApartmentNumber: null,
      PostalCode: '45-208',
      Town: 'Warsaw',
      PhoneNumber: 228235591,
      DateOfBirth: new Date("1991-02-10"),
      Age: 31
    }
  ];
  constructor() { }

  ngOnInit(): void {

  }

}
