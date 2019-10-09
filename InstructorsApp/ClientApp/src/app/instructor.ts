export class Instructor {
  id: number;
  firstName: string;
  middleName: string;
  lastName: string;

  constructor(firstName: string, middleName: string, lastName: string) {
    this.firstName = firstName;
    this.middleName = middleName;
    this.lastName = lastName;
  }
}
