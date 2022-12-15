export interface Employee {
    id: string;
    firstName: string;
    lastName: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber: number | null;
    postalCode: string;
    town: string;
    phoneNumber: string;
    dateOfBirth: Date;
    age: number;
}