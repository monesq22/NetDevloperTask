import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BusinessCardService } from '../business-card.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxFileDropEntry, FileSystemFileEntry } from 'ngx-file-drop';

@Component({
  selector: 'app-add-business-card',
  templateUrl: './add-business-card.component.html',
  styleUrls: ['./add-business-card.component.css']
})
export class AddBusinessCardComponent implements OnInit {
  businessCardForm: FormGroup;
  photo: string | null = null;
  photoPreview: string | ArrayBuffer | null = null;
  previewData: any[] = [];
  selectedFile: File | null = null;
  importedData: any[] = [];

  constructor(
    private fb: FormBuilder,
    private businessCardService: BusinessCardService,
    private snackBar: MatSnackBar
  ) {
    this.businessCardForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      address: ['', [Validators.required, Validators.maxLength(200)]],
      photo: ''
    });
  }

  ngOnInit(): void {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file: File = input.files[0];
      const reader = new FileReader();

      reader.onload = () => {
        this.photoPreview = reader.result;
        this.photo = reader.result as string;
        this.businessCardForm.patchValue({ photo: this.photo });
      };

      reader.readAsDataURL(file);
    }
  }

  onSubmit(): void {
    if (this.businessCardForm.valid) {
      const businessCardData = this.businessCardForm.value;

      this.businessCardService.createBusinessCard(businessCardData).subscribe(
        () => {
          this.businessCardForm.reset();
          this.photoPreview = null;
          this.showNotification('Business card added successfully');
        },
        () => {
          this.showNotification('Error adding business card', 'error');
        }
      );
    } else {
      this.businessCardForm.markAllAsTouched();
    }
  }

  onFileImportSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.selectedFile = input.files[0];
      this.importFile();
    }
  }

  onFileDropped(files: NgxFileDropEntry[]): void {
    for (const droppedFile of files) {
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.selectedFile = file;
          this.importFile();
        });
      }
    }
  }

  importFile(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      this.businessCardService.importBusinessCards(formData).subscribe(
        (response: any) => {
          this.previewData = response;
          this.showNotification('File imported successfully');
        },
        () => {
          this.showNotification('Error importing file', 'error');
        }
      );
    }
  }

  saveImportedCards(): void {
    if (this.previewData.length > 0) {
      this.previewData.forEach(card => {
        this.businessCardService.createBusinessCard(card).subscribe(
          () => {
            this.showNotification('Business card saved');
          },
          () => {
            this.showNotification('Error saving business card', 'error');
          }
        );
      });
      this.previewData = [];
    }
  }

  private showNotification(message: string, type: string = 'success'): void {
    this.snackBar.open(message, '', {
      duration: 3000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: type === 'error' ? 'snack-bar-error' : 'snack-bar-success'
    });
  }
  
}
