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
    return this.http.get<Instructor[]>('api/instructor');
  }

  delete(id: number) : Observable<any> {
    return this.http.delete('api/instructor/' + id);
  }
}
