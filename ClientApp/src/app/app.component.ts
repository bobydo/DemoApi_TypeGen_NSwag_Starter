import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div class="app-container">
      <h1>Demo API - Angular Client</h1>
      <app-students></app-students>
    </div>
  `,
  styles: [`
    .app-container {
      padding: 20px;
    }
    h1 {
      text-align: center;
      color: #333;
      margin-bottom: 30px;
    }
  `]
})
export class AppComponent {
  title = 'demo-api-client';
}
