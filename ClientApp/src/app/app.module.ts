import { BooksService } from './services/books.service';
import { UserService } from './services/user.service';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BookFormComponent } from './book-form/book-form.component';
import { BooksComponent } from './books/books.component';
import { BookComponent } from './book/book.component';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    BookFormComponent,
    BooksComponent,
    BookComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot({
      progressBar: true
    }),
    RouterModule.forRoot([
      { path: '', component: BooksComponent, pathMatch: 'full' },
      { path: 'books/new', component: BookFormComponent },
      { path: 'book/:id', component: BookComponent },
      { path: 'books', component: BooksComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'user', component: UserComponent, children: [
        {path: 'registration', component: RegistrationComponent},
        {path: 'login', component: LoginComponent}
      ] },
      { path: '**', redirectTo: 'Home' }
    ])
  ],
  providers: [
    BooksService,
    UserService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
