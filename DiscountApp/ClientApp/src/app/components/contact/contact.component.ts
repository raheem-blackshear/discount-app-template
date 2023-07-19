
import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { AlertService, MessageSeverity } from '../../services/alert.service';
import { AccountService } from '../../services/account.service';
import { Contact } from '../../models/contact.model';
@Component({
    selector: 'contact',
    templateUrl: './contact.component.html',
    styleUrls: ['./contact.component.css'],
    animations: [fadeInOut]
})
export class ContactComponent {
    title = 'Contact-us';

    private contact: Contact = new Contact();
    name: string = '';
    email: string = '';
    mobile: string = '';
    subject: string = '';
    message: string = '';

    constructor(private alertService: AlertService, private accountService: AccountService) {
    }

    save(name, email, mobile, subject, message): void {
        this.contact.name = name;
        this.contact.email = email;
        this.contact.mobile = mobile;
        this.contact.subject = subject;
        this.contact.message = message;
        this.accountService.submitContact(this.contact).subscribe(user => this.saveSuccessHelper(), error => this.saveFailedHelper(error));

    }
    private saveSuccessHelper() {

        this.alertService.showMessage("Success", `submit form  successfully`, MessageSeverity.success);
        this.name = '';
        this.email = '';
        this.mobile = '';
        this.subject = '';
        this.message = '';
    }

    private saveFailedHelper(error: any) {       
        this.alertService.showStickyMessage("Save Error", "The below errors occured whilst saving your changes:", MessageSeverity.error, error);
        this.alertService.showStickyMessage(error, null, MessageSeverity.error);       
    }
}
