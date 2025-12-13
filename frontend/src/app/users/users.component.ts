import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, provideHttpClient } from '@angular/common/http';
import { ApiClient, User } from '../services/api-client';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  private apiClient = inject(ApiClient);
  
  users: User[] = [];
  loading = false;
  error: string | null = null;

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;
    this.error = null;
    
    this.apiClient.users_GetUsers().subscribe({
      next: (users: User[]) => {
        this.users = users;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load users: ' + err.message;
        this.loading = false;
      }
    });
  }
}
