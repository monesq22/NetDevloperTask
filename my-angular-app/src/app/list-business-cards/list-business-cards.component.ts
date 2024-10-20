import { Component, OnInit } from '@angular/core';
import { BusinessCardService } from '../business-card.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-list-business-cards',
  templateUrl: './list-business-cards.component.html',
  styleUrls: ['./list-business-cards.component.css']
})
export class ListBusinessCardsComponent implements OnInit {
  businessCards: any[] = [];
  filteredBusinessCards: any[] = [];
  searchParams: any = {
    name: '',
    email: '',
    phone: '',
    dateOfBirth: '',
    gender: '',
    address: ''
  };

  constructor(private businessCardService: BusinessCardService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.loadBusinessCards();
  }

  loadBusinessCards(): void {
    this.businessCardService.getBusinessCards().subscribe(
      (data) => {
        this.businessCards = data;
        this.filteredBusinessCards = data;
      },
      (error) => {
        this.showNotification('Error fetching business cards', 'error');
      }
    );
  }

  filterBusinessCards(): void {
    this.filteredBusinessCards = this.businessCards.filter(card => {
      return (
        (this.searchParams.name === '' || card.name.toLowerCase().includes(this.searchParams.name.toLowerCase())) &&
        (this.searchParams.email === '' || card.email.toLowerCase().includes(this.searchParams.email.toLowerCase())) &&
        (this.searchParams.phone === '' || card.phone.includes(this.searchParams.phone)) &&
        (this.searchParams.dateOfBirth === '' || card.dateOfBirth === this.searchParams.dateOfBirth) &&
        (this.searchParams.gender === '' || card.gender.toLowerCase() === this.searchParams.gender.toLowerCase()) &&
        (this.searchParams.address === '' || card.address.toLowerCase().includes(this.searchParams.address.toLowerCase()))
      );
    });
  }

  clearFilters(): void {
    this.searchParams = { name: '', email: '', phone: '', dateOfBirth: '', gender: '', address: '' };
    this.filteredBusinessCards = this.businessCards;
  }

  deleteBusinessCard(id: number): void {
    if (confirm('Are you sure you want to delete this business card?')) {
      this.businessCardService.deleteBusinessCard(id).subscribe(
        () => {
          this.showNotification('Business card deleted successfully');
          this.loadBusinessCards();
        },
        (error) => {
          this.showNotification('Error deleting business card', 'error');
        }
      );
    }
  }

  exportBusinessCardsAsXml(): void {
    this.exportBusinessCards('xml');
  }

  exportBusinessCardsAsCsv(): void {
    this.exportBusinessCards('csv');
  }

  private exportBusinessCards(fileType: string): void {
    this.businessCardService.exportBusinessCards(fileType).subscribe(
      (blob: Blob) => {
        const fileName = fileType === 'xml' ? 'business-cards.xml' : 'business-cards.csv';
        this.downloadFile(blob, fileName, fileType);
      },
      (error: any) => {
        this.showNotification(`Error exporting business cards as ${fileType.toUpperCase()}`, 'error');
      }
    );
  }
  
  private downloadFile(blob: Blob, fileName: string, fileType: string): void {
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
    
    this.showNotification(`File ${fileName} downloaded successfully`, 'success');
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
