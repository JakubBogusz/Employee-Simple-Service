export interface Employee {
    Id: string;
    FirstName: string;
    LastName: string;
    StreetName: string;
    HouseNumber: string;
    ApartmentNumber: number | null;
    PostalCode: string;
    Town: string;
    PhoneNumber: number;
    DateOfBirth: Date;
    Age: number;
}