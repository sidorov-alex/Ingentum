import { Component, OnInit } from '@angular/core';
import { Instructor } from 'src/app/instructor';
import { InstructorService } from 'src/app/instructor.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  private instructorList: Instructor[];

  private selectedItem: Instructor | null;

  constructor(
    private instructorService: InstructorService) { }

  ngOnInit() {
    this.instructorService.getList()
      .subscribe(list => this.instructorList = list);
  }

  private onRowClick(item: Instructor) {
    this.selectedItem = item;
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
}
