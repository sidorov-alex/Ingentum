import { Component, OnInit } from '@angular/core';
import { Instructor } from 'src/app/instructor';
import { InstructorService } from 'src/app/instructor.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  private instructorList: Instructor[];
  private _selectedItem: Instructor | null = null;
   
  model: Instructor = new Instructor("", "", "");

  constructor(
    private instructorService: InstructorService) { }

  ngOnInit() {
    this.instructorService.getList()
      .subscribe(list => this.instructorList = list);
  }

  private get selectedItem() {
    return this._selectedItem;
  }

  private set selectedItem(item: Instructor) {
    this._selectedItem = item;

    this.onSelectedItemChanged();
  }

  private onRowClick(item: Instructor) {
    if (this.selectedItem === item) {
      this.selectedItem = null;
    }
    else {
      this.selectedItem = item;
    }
  }

  private onDeleteClick(item: Instructor) {
    this.instructorService.delete(item.id)
      .subscribe(() => this.removeItem(item));
  }

  private removeItem(item: Instructor) {
    for (var i = 0; i < this.instructorList.length; i++) {
      if (this.instructorList[i] === item) {
        this.instructorList.splice(i, 1);
      }
    }

    if (this.selectedItem === item) {
      this.selectedItem = null;
    }
  }

  private onSelectedItemChanged() {
    if (this.selectedItem === null) {
      this.model.id = 0;
      this.model.firstName = "";
      this.model.middleName = "";
      this.model.lastName = "";
    }
    else {
      this.model.id = this.selectedItem.id;
      this.model.firstName = this.selectedItem.firstName;
      this.model.middleName = this.selectedItem.middleName;
      this.model.lastName = this.selectedItem.lastName;
    }
  }

  private onSaveClick() {
    if (this.model.id) {
      let item = this.selectedItem;

      this.instructorService.update(this.model.id, this.model)
        .subscribe(() => {
          item.firstName = this.model.firstName;
          item.middleName = this.model.middleName;
          item.lastName = this.model.lastName;
        });
    }
    else {
      this.instructorService.add(this.model)
        .subscribe(item => {
          this.instructorList.push(item)
          this.selectedItem = item;
        });
    }
  }
}
