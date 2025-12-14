import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentDto } from '../models';
import { API_BASE_URL, BaseApiService } from '../core';

@Injectable()
export class StudentsService extends BaseApiService {
    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(http, baseUrl ?? "");
    }

    getStudents(): Observable<StudentDto[]> {
        return this.get<StudentDto[]>("/api/Students");
    }

    // Easy to add more methods:
    // getStudent(id: number): Observable<StudentDto> {
    //     return this.get<StudentDto>(`/api/Students/${id}`);
    // }
    //
    // createStudent(student: StudentDto): Observable<StudentDto> {
    //     return this.post<StudentDto>("/api/Students", student);
    // }
    //
    // updateStudent(id: number, student: StudentDto): Observable<StudentDto> {
    //     return this.put<StudentDto>(`/api/Students/${id}`, student);
    // }
    //
    // deleteStudent(id: number): Observable<void> {
    //     return this.delete<void>(`/api/Students/${id}`);
    // }
}
