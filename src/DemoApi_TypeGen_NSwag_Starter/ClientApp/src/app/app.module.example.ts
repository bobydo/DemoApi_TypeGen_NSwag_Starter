// Example: How to configure the Angular module with the generated services
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { StudentsComponent } from './components/students';
import { StudentsService } from './services';
import { API_BASE_URL } from './core';

@NgModule({
  declarations: [
    AppComponent,
    StudentsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [
    StudentsService,
    { provide: API_BASE_URL, useValue: 'http://localhost:5098' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
