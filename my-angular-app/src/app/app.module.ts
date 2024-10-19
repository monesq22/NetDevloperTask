import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';  
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddBusinessCardComponent } from './add-business-card/add-business-card.component';
import { ListBusinessCardsComponent } from './list-business-cards/list-business-cards.component';
import { BusinessCardService } from './business-card.service';  
import { HttpClientModule } from '@angular/common/http'; 
import { NgxFileDropModule } from 'ngx-file-drop';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';  
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    AppComponent,
    AddBusinessCardComponent,
    ListBusinessCardsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxFileDropModule, BrowserAnimationsModule,
    MatSnackBarModule,
    MatButtonModule
  ],
  providers: [
    BusinessCardService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
