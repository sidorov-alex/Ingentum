import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Instructor } from 'src/app/instructor';

@Injectable({
  providedIn: 'root'
})
export class InstructorService {

  constructor(
    private http: HttpClient) { }

  getList(): Observable<Instructor[]>{
    return this.http.get<Instructor[]>("api/instructor");
  }

  add(item: Instructor): Observable<Instructor> {
    return this.http.post<Instructor>("api/instructor", item);
  }

  update(id: number, item: Instructor) {
    return this.http.put<Instructor>("api/instructor/" + id, item);
  }

  delete(id: number) : Observable<any> {
    return this.http.delete("api/instructor/" + id);
  }
}
