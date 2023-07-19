import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Age } from '../../../models/age.model';
import { SetupService } from "../../../services/setup.service";
import { Utilities } from "../../../services/utilities";
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { AppTranslationService } from "../../../services/app-translation.service";

@Component({
  selector: 'age-info',
  templateUrl: './age-info.component.html',
  styleUrls: ['./age-info.component.css']
})
export class AgeInfoComponent implements OnInit {

    public changesSavedCallback: () => void;
    public changesFailedCallback: () => void;
    public changesCancelledCallback: () => void;


    private isEditMode = false;
    private isNewUser = false;
    private isSaving = false;
    private isEditingSelf = false;
    private showValidationErrors = false;
    private editingAgeName: string;
    private uniqueId: string = Utilities.uniqueId();
    private age: Age = new Age();
    private ageEdit: Age = new Age();
    public formResetToggle = true;

    @Input()
    isViewOnly: boolean;

    @Input()
    isGeneralEditor = false;

    @ViewChild('f')
    private form;

    constructor(private alertService: AlertService, private translationService: AppTranslationService, private setupService: SetupService) {

    }

    ngOnInit() {
        if (!this.isGeneralEditor) {
            this.loadCurrentAgeData();
        }
    }


    private loadCurrentAgeData() {
        this.alertService.startLoadingMessage();
        this.setupService.getAge().subscribe(age => this.onCurrentAgeDataLoadSuccessful(age), error => this.onCurrentAgeDataLoadFailed(error));
    }


    private onCurrentAgeDataLoadSuccessful(age: Age) {
        this.alertService.stopLoadingMessage();
        this.age = age;
    }


    private onCurrentAgeDataLoadFailed(error: any) {
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve age data from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);

        this.age = new Age();
    }


    resetForm(replace = false) {

        if (!replace) {
            this.form.reset();
        }
        else {
            this.formResetToggle = false;

            setTimeout(() => {
                this.formResetToggle = true;
            });
        }
    }


    newAge() {
        this.isGeneralEditor = true;
        this.isNewUser = true;

        this.editingAgeName = null;
        this.age = this.ageEdit = new Age();
        this.edit();

        return this.ageEdit;
    }


    editAge(age: Age) {
        if (age) {
            this.isGeneralEditor = true;
            this.isNewUser = false;

            this.editingAgeName = age.ageName;
            this.age = new Age();
            this.ageEdit = new Age();
            Object.assign(this.age, age);
            Object.assign(this.ageEdit, age);
            this.edit();

            return this.ageEdit;
        }
        else {
            return this.newAge();
        }
    }


    private edit() {
        if (!this.isGeneralEditor) {
            this.isEditingSelf = true;
            this.ageEdit = new Age();
            Object.assign(this.ageEdit, this.age);
        }
        else {
            if (!this.ageEdit)
                this.ageEdit = new Age();

             //this.isEditingSelf = this.setupService.currentUser ? this.ageEdit.ageId == this.setupService.currentUser.id : false;
        }

        this.isEditMode = true;
        this.showValidationErrors = true;

    }


    displayAge(age: Age) {

        this.age = new Age();
        Object.assign(this.age, age);

        this.isEditMode = false;
    }


    private showErrorAlert(caption: string, message: string) {
        this.alertService.showMessage(caption, message, MessageSeverity.error);
    }



    private save() {
        this.isSaving = true;
        this.alertService.startLoadingMessage("Saving changes...");

        if (this.isNewUser) {
            this.setupService.newAge(this.ageEdit).subscribe(type => this.saveSuccessHelper(type), error => this.saveFailedHelper(error));
        }
        else {
            this.setupService.updateAge(this.ageEdit).subscribe(response => this.saveSuccessHelper(), error => this.saveFailedHelper(error));
        }
    }

    private saveSuccessHelper(type?: Age) {

        if (type)
            Object.assign(this.ageEdit, type);

        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.showValidationErrors = false;

        Object.assign(this.age, this.ageEdit);
        this.ageEdit = new Age();
        this.resetForm();


        if (this.isGeneralEditor) {
            if (this.isNewUser)
                this.alertService.showMessage("Success", `Age \"${this.age.ageName}\" was created successfully`, MessageSeverity.success);
            else if (!this.isEditingSelf)
                this.alertService.showMessage("Success", `Changes to user \"${this.age.ageName}\" was saved successfully`, MessageSeverity.success);
        }

        if (this.isEditingSelf) {
            this.alertService.showMessage("Success", "Changes to your Age was saved successfully", MessageSeverity.success);

        }

        this.isEditMode = false;


        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }


    private saveFailedHelper(error: any) {
        this.isSaving = false;
        this.alertService.stopLoadingMessage();
        this.alertService.showStickyMessage("Save Error", "The below errors occured whilst saving your changes:", MessageSeverity.error, error);
        this.alertService.showStickyMessage(error, null, MessageSeverity.error);

        if (this.changesFailedCallback)
            this.changesFailedCallback();
    }



    private cancel() {
        if (this.isGeneralEditor)
            this.ageEdit = this.age = new Age();
        else
            this.ageEdit = new Age();

        this.showValidationErrors = false;
        this.resetForm();

        this.alertService.showMessage("Cancelled", "Operation cancelled by user", MessageSeverity.default);
        this.alertService.resetStickyMessage();

        if (!this.isGeneralEditor)
            this.isEditMode = false;

        if (this.changesCancelledCallback)
            this.changesCancelledCallback();
    }


    private close() {
        this.ageEdit = this.age = new Age();
        this.showValidationErrors = false;
        this.resetForm();
        this.isEditMode = false;

        if (this.changesSavedCallback)
            this.changesSavedCallback();
    }





}
