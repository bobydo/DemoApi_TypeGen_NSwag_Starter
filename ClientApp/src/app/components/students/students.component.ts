import { Component, OnInit } from '@angular/core';
import { StudentsService } from '../../services';
import { StudentDto } from '../../models';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  students: StudentDto[] = [];
  loading = false;
  error: string | null = null;

  constructor(private studentsService: StudentsService) { }

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.loading = true;
    this.error = null;

    this.studentsService.getStudents()
      .subscribe({
        next: (data: StudentDto[]) => {
          this.students = data;
          this.loading = false;
        },
        error: (err: any) => {
          this.error = 'Failed to load students';
          this.loading = false;
          console.error('Error loading students:', err);
        }
      });
  }

  refresh(): void {
    this.loadStudents();
  }
}
